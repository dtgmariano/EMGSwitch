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
#define xsize_auiChAData 10
#define maxsize_aucTXBuffer 50
#define size_header_TXBuffer 5
#define BAUDRATE 9600                            // BaudRate

/*Variables*/
const int aRef = 512;

volatile unsigned int uiADC_Data = 0;                         //ADC Channel A current value
volatile unsigned int uiChA_Data = 0;                                  //Channel A Data
volatile unsigned int auiChA_Data[maxsize_auiChAData];        //Vector for Channel A Data
unsigned int idxVecChA = 0;                                   //Index of the Vector for Channel A
bool bOverflow;                                               //VecDataChannel overflowed!
unsigned int counter = 0;
unsigned char* aucTXBuffer;
int size_aucTXBuffer = 0;
unsigned int idxEpoch = 0;                                    //Index of Epoch


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
  digitalWrite(LED1, LOW);                 //Setup LED1 state

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
  if (idxVecChA >= xsize_auiChAData) {
    Toggle_LED1();
    SendData();
  }
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
  uiADC_Data = analogRead(0);

  if (RECTIFY == 1) uiChA_Data = RectifySignal(uiADC_Data);
  else uiChA_Data = uiADC_Data;

  //auiChA_Data[idxVecChA++] = uiChA_Data;
  auiChA_Data[idxVecChA++] = counter++;

  if (counter >= 1000) counter = 0;

  if (idxVecChA >= maxsize_auiChAData)
  {
    bOverflow = true;   /*Reached the array max size*/
    idxVecChA = 0;
  }
}

/****************************************************/
/*  Function name: SendData                         */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action  :  Send data                     .    */
/*    Observation: For a n bytes buffer:            */
/*    byte[0] : header package                      */
/*    byte[1] : header package                      */
/*    byte[2] : epoch                               */
/*    byte[3] : size of auiChA_Data                 */
/*    byte[4] : auiChA_Data[0]_msb                  */
/*    byte[5] : auiChA_Data[0]_lsb                  */
/*    byte[6] : auiChA_Data[1]_msb                  */
/*    byte[7] : auiChA_Data[1]_lsb                  */
/*      ...                                         */
/*    byte[n-3]: auiChA_Data[k]_msb                 */
/*    byte[n-2]: auiChA_Data[k]_lsb                 */
/*    byte[n-1]: end package                        */
/****************************************************/
void SendData()
{
  unsigned int size_aucTXBuffer = (idxVecChA * 2) + size_header_TXBuffer;
  aucTXBuffer = (unsigned char*) realloc(aucTXBuffer, size_aucTXBuffer * sizeof(unsigned char));

  aucTXBuffer[0] = 0b0000000000110011;  //Header 1
  aucTXBuffer[1] = 0b0000000011001100;  //Header 2
  aucTXBuffer[2] = ((unsigned char)(idxEpoch & 0b0000000011111111));
  aucTXBuffer[3] = ((unsigned char)(idxVecChA & 0b0000000011111111));

  int idx_txbuf = 4;
  for (int i = 0; i < idxVecChA; i++)
  {
    aucTXBuffer[idx_txbuf++] = ((unsigned char)((auiChA_Data[i] & 0b0000001111100000) >> 5) | 0b0000000011100000); //hb 111XXXXX
    aucTXBuffer[idx_txbuf++] = ((unsigned char)((auiChA_Data[i] & 0b0000000000011111)));                           //lb 000XXXXX
  }
  aucTXBuffer[idx_txbuf] = 0b0000000010011011; //foot

  idxEpoch++;
  if (idxEpoch >= 200) idxEpoch = 0;
  idxVecChA = 0;

  for (int i = 0; i < size_aucTXBuffer; i++)
  {
    Serial.write(aucTXBuffer[i]);
    Serial.flush();
  }
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
  if (uiPoint < aRef) return 2 * aRef - uiPoint;
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
  if (digitalRead(LED1) == HIGH) digitalWrite(LED1, LOW);
  else digitalWrite(LED1, HIGH);
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
