/**********************************************/
/* Title: Arduino Data Acquisition            */
/* Version: 3.0                               */
/* Date: 21/07/2015                           */
/* Author: Daniel T. G. Mariano               */
/* Last Modification: 17/06/2016              */
/* Changed by: Daniel T. G. Mariano           */
/* Email: dtgmariano@gmail.com                */
/* Description: Read analog data and sends    */
/* them through the serial port.              */
/**********************************************/

#include <FlexiTimer2.h>

/*All definitions*/
#define LED1 13                                   // LED1 pin
#define SAMPFREQ 128                              // ADC sampling rate 256 Hz
#define TIMER2VAL (1024/(SAMPFREQ))               // Set sampling frequency
#define RECTIFY 0                                 // 0 No action; 1 Recify EMG Signal based on ARef.
#define maxsize_auiChAData 50
#define maxsize_aucTXBuffer 50
#define BAUDRATE 19200                            // BaudRate

/*Variables*/
const int aRef = 512;

volatile unsigned int uiADC_Data = 0;                         //ADC Channel A current value
volatile unsigned int uiChA_Data = 0;                                  //Channel A Data
volatile unsigned int auiChA_Data[maxsize_auiChAData];        //Vector for Channel A Data
unsigned int idxVecChA = 0;                                   //Index of the Vector for Channel A
bool bOverflow;                                               //VecDataChannel overflowed!

volatile unsigned char* aucTXBuffer;
int size_aucTXBuffer = 0;
unsigned int idxEpoch = 0;                                    //Index of Epoch

//volatile unsigned char TXBuf[VS];                           //Transmission packet


/****************************************************/
/*  Function name: setup                            */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action: Initializes all peripherals           */
/****************************************************/
void setup() {
   noInterrupts();
   pinMode(LED1, OUTPUT);                   //Setup LED1 direction
   digitalWrite(LED1,LOW);                  //Setup LED1 state

   //TXBuf[0] = 0xa5;    //CH1 High Byte
   //for(int iCount = 0; iCount < VS; iCount++)
   //{
   //   if(i%2==0) TXBuf[i] = 0x02; //CH1 Low Byte
   //   else  TXBuf[i] = 0x00; //CH1 Low Byte
   //}

   idxVecChA = 0;
   idxEpoch = 0;
   bOverflow = false;

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
    if(idxVecChA>=10) {Toggle_LED1(); SendData();}
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
    uiADCDataChannelA = analogRead(0);

    if(RECTIFY == 1) uiDataChannelA = RectifySignal(uiADCDataChannelA);
    else uiDataChannelA = uiADCDataChannelA;

    uiVecDataChannelA[idxVecChA++] = uiDataChannelA;
    if(idxVecChA>=vecA_Size)
    {
      bOverflow = true;
      idxVecA = 0;
    }
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

    aucTXBuffer = (unsigned int*) realloc(aucTXBuffer, size * sizeof(unsigned int));
    size_aucTXBuffer =

    TXBuf[0] = 0b0000000000110011  //Header
    TXBuf[1] = 0b0000000001010101  //End

    TXBuf[2] = ((unsigned char)((idxEpoch & 0b0000001111100000) >> 5) | 0b0000000000000000); //Package
    TXBuf[3] = ((unsigned char)((idxEpoch & 0b0000000000011111)| 0b0000000000000000);

    idxEpoch++;
    if(idxEpoch>=1000) idxEpoch = 0;

    for(int i=0; i<idxVecA; i+=2)
    {
        TXBuf[(i_Idx_TXBuf++)+1] = ((unsigned char)((uiVecDataChannelA[i] & 0b0000001111100000) >> 5) | 0b0000000011100000); //hb 111XXXXX
        TXBuf[(i_Idx_TXBuf++)+1] = ((unsigned char)((uiVecDataChannelA[i] & 0b0000000000011111)));                           //lb 000XXXXX
    }
    idxVecA = 0;
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
}
