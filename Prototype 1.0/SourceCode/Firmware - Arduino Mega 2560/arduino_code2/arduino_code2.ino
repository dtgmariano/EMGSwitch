/**********************************************/
/* Title: EMG Olimex Shield Firmware          */
/* Version: 3.0                               */
/* Date: 21/07/2015                           */
/* Author: Daniel T. G. Mariano               */
/* Email: dtgmariano@gmail.com                */
/* Description: Code to send a 10 bits signal */
/**********************************************/

#include <FlexiTimer2.h>

/*All definitions*/
#define LED1 13
#define SAMPFREQ 128                      // ADC sampling rate 256 Hz
#define TIMER2VAL (1024/(SAMPFREQ))       // Set sampling frequency
#define RECTIFY 1
#define CAsize 10 // Size of the Vector
#define BufferSize 22  //Size of Serial Buffer

#define BAUDRATE 19200

/*Variables*/
const int aRef = 512;

volatile unsigned int uiADCDataChannelA = 0;      //ADC Channel A current value
unsigned int uiDataChannelA = 0;                  //Channel A Data
volatile unsigned int uiVecDataChannelA[CAsize];  //Vector for Channel A Data
unsigned int idxVecA = 0;                         //Index of Vector A

volatile unsigned char TXBuf[VS];          //Transmission packet
bool bOverflow;
//unsigned int readings[numReadings];       // the readings from the analog input
//int index = 0;                            // the index of the current reading
//unsigned total = 0;                       // the running total
//unsigned average = 0;                     // the average

/****************************************************/
/*  Function name: Toggle_LED1                      */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action: Switches-over LED1.                   */
/****************************************************/
void Toggle_LED1(void)
{
    if(digitalRead(LED1)==HIGH) digitalWrite(LED1,LOW);
    else digitalWrite(LED1,HIGH);
}

/****************************************************/
/*  Function name: setup                            */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action: Initializes all peripherals           */
/****************************************************/
void setup() {
   noInterrupts();
   pinMode(LED1, OUTPUT);  //Setup LED1 direction
   digitalWrite(LED1,LOW); //Setup LED1 state

   //TXBuf[0] = 0xa5;    //CH1 High Byte
   for(int iCount = 0; iCount < VS; iCount++)
   {
     if(i%2==0) TXBuf[i] = 0x02; //CH1 Low Byte
     else  TXBuf[i] = 0x00; //CH1 Low Byte
   }

   idxVecA = 0;

   /*for (int thisReading = 0; thisReading < numReadings; thisReading++)
   {
      readings[thisReading] = 0;
   }*/

   Serial.begin(BAUDRATE);
   FlexiTimer2::set(TIMER2VAL, DataAquisition_ISR);
   FlexiTimer2::start();
   interrupts();
}

/****************************************************/
/*  Function name: loop                             */
/*  Parameters                                      */
/*     Input:  No                                   */
/*    Output:  No                                   */
/*    Action:  Main                                 */
/****************************************************/
void loop()
{
    // put your main code here, to run repeatedly:
    //__asm__ __volatile__ ("sleep");
    if(idxVecA>=10) SendData();
}

/****************************************************/
/*  Function name: DataAquisition_ISR              */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action: Determines ADC sampling frequency.    */
/****************************************************/
void DataAquisition_ISR()
{
    Toggle_LED1();
    uiADCDataChannelA = analogRead(0);
    //if(RECTIFY == 1) uiDataChannelA = RectifySignal(uiADCDataChannelA);
    //else uiDataChannelA = uiADCDataChannelA;
    uiVecDataChannelA[idxVecA++] = uiDataChannelA;
    if(idxVecA>CAsize)
    {
      bOverflow = true;
      idxVecA = 0;
    }
    //TXBuf[0] = ((unsigned char)((ADC_Value & 0b0000001111100000) >> 5) | 0b0000000011100000); //hb 111XXXXX
    //TXBuf[1] = ((unsigned char)((ADC_Value & 0b0000000000011111)));                           //lb 000XXXXX
    //Serial.write(TXBuf[0]);
    //Serial.write(TXBuf[1]);
}

/****************************************************/
/*  Function name: RectifySignal                    */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action  :  Send data                     .    */
/****************************************************/
unsigned int RectifySignal(unsigned int uiPoint)
{
    if(uiPoint < aRef) return 2 * aRef - uiPoint;
    else return uiPoint;
}

/****************************************************/
/*  Function name: SendData                         */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action  :  Send data                     .    */
/****************************************************/
void SendData()
{
    int i_Idx_TXBuf = 0;
    TXBuf[0] = 0b0000001100110011   //Header
    for(int i=0; i<idxVecA; i+=2)
    {
        TXBuf[(i_Idx_TXBuf++)+1] = ((unsigned char)((uiVecDataChannelA[i] & 0b0000001111100000) >> 5) | 0b0000000011100000); //hb 111XXXXX
        TXBuf[(i_Idx_TXBuf++)+1] = ((unsigned char)((uiVecDataChannelA[i] & 0b0000000000011111)));                           //lb 000XXXXX
    }
    TXBuf[i_Idx_TXBuf+1] = 0b0000110011001100 //End
    idxVecA = 0;
}

void filt()
{
  //    /*Smoothing and Rectifying*/
  //    // subtract the last reading:
  //    total= total - readings[index];
  //    // read from the sensor:
  //
  //    ADC_Value = analogRead(0);
  //    if(ADC_Value < aRef)
  //    {
  //       readings[index] = 2 * aRef - ADC_Value;
  //    }
  //
  //    // add the reading to the total:
  //    total = total + readings[index];
  //    // advance to the next position in the array:
  //    index = index + 1;
  //
  //    // if we're at the end of the array...
  //    if (index >= numReadings)
  //    // ...wrap around to the beginning:
  //    index = 0;
  //
  //    // calculate the average:
  //    average = total / numReadings;
  //
  //    TXBuf[0] = ((unsigned char)((average & 0b0000001111100000) >> 5) | 0b0000000011100000); //hb 111XXXXX
  //    TXBuf[1] = ((unsigned char)((average & 0b0000000000011111)));                           //lb 000XXXXX
  //    Serial.write(TXBuf[0]);
  //    Serial.write(TXBuf[1]);
}
