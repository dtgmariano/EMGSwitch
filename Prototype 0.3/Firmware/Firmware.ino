/**********************************************/
/* Title: Arduino Data Acquisition            */
/* Version: 4.0                               */
/* Date: 02/09/2016                           */
/* Author: Daniel T. G. Mariano               */
/* Last Modification: 02/09/2016              */
/* Changed by: Daniel T. G. Mariano           */
/* Email: dtgmariano@gmail.com                */
/* Description: Read analog data and sends    */
/* them through the serial port.              */
/**********************************************/

/*All includes*/
#include <FlexiTimer2.h>

/*All definitions*/
#define baudrate 19200
#define LED1 13
#define samplingfrequency 4
#define timer2value (1024/samplingfrequency)
#define size_auiChA  50
#define limit_auiChA  2
#define TEST 1
#define RECTIFY 0
#define FILTER 0

/*All variables*/
unsigned int uiADC = 0;
unsigned int uiChA = 0;
volatile unsigned int auiChA[size_auiChA];
unsigned int uiIdx_auiChA = 0;
bool bOverflow_auiChA;

volatile unsigned int auiSin[90] = {
256, 344, 421, 478, 508, 508, 478, 421, 344, 
256, 168, 91, 34, 4, 4, 34, 91, 168, 
256, 344, 421, 478, 508, 508, 478, 421, 344, 
256, 168, 91, 34, 4, 4, 34, 91, 168, 
256, 344, 421, 478, 508, 508, 478, 421, 344, 
256, 168, 91, 34, 4, 4, 34, 91, 168, 
256, 344, 421, 478, 508, 508, 478, 421, 344, 
256, 168, 91, 34, 4, 4, 34, 91, 168, 
256, 344, 421, 478, 508, 508, 478, 421, 344, 
256, 168, 91, 34, 4, 4, 34, 91, 168};

unsigned int uiCounterTest = 0;

unsigned char* aucTXBuffer;
unsigned int uiEpoch_aucTXBuffer;

/****************************************************/
/*  Function name: setup                            */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action: Initializes all peripherals           */
/****************************************************/
void setup() 
{
  noInterrupts();
  pinMode(LED1, OUTPUT);                   //Setup LED1 direction
  digitalWrite(LED1, LOW);                 //Setup LED1 state

  uiIdx_auiChA = 0;
  bOverflow_auiChA = false;
  uiEpoch_aucTXBuffer = 0;

  Serial.begin(baudrate);
  if(TEST)
    FlexiTimer2::set(timer2value, Test_ISR);
  else
    FlexiTimer2::set(timer2value, DataAquisition_ISR);
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
  
  if (uiIdx_auiChA >= limit_auiChA) 
  {
    Toggle_LED1();
    SendData();
  }
}

/****************************************************/
/*  Function name: DataAquisition_ISR               */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action: Determines ADC sampling frequency.    */
/****************************************************/
void DataAquisition_ISR()
{
  uiADC = analogRead(0);
  uiChA = SignalProcess(uiADC);
  auiChA[uiIdx_auiChA++] = uiChA;

  if (uiIdx_auiChA >= size_auiChA)
  {
    bOverflow_auiChA = true;   /*Reached the array max size*/
    uiIdx_auiChA = 0;
  }
}

/****************************************************/
/*  Function name: Test_ISR                         */
/*  Parameters                                      */
/*    Input   :  No                                 */
/*    Output  :  No                                 */
/*    Action: Determines ADC sampling frequency.    */
/****************************************************/
void Test_ISR()
{
  uiChA = auiSin[uiCounterTest++];
  
  if (uiCounterTest >= 18)
    uiCounterTest = 0;
  
  auiChA[uiIdx_auiChA++] = uiChA;

  if (uiIdx_auiChA >= size_auiChA)
  {
    bOverflow_auiChA = true;   /*Reached the array max size*/
    uiIdx_auiChA = 0;
  }
}

/************************************************************/
/*  Function name: SendData                                 */
/*  Parameters                                              */
/*    Input   :  No                                         */
/*    Output  :  No                                         */
/*    Action  :  Send data                                  */
/*    Observation: For a n bytes buffer:                    */
/*    byte[0]  : header1       byte[1] : header2            */
/*    byte[2]  : epoch         byte[3] : size of auiChA     */
/*    byte[4]  : auiChA overflow                            */
/*    byte[5]  : Data[0] MSB   byte[6] : Data[0] LSB        */
/*    byte[7]  : Data[0] MSB   byte[8] : Data[0] LSB        */
/*      ...                                                 */
/*    byte[n-3]: Data[k] MSB  byte[n-2]: Data[k] LSB        */
/*    byte[n-1]: end package                                */
/************************************************************/
void SendData()
{
  unsigned int size_header_TXBuffer = 5;                                        //Header size is for 4
  unsigned int size_aucTXBuffer = (uiIdx_auiChA * 2) + size_header_TXBuffer;    //Calculates the size for aucTXBuffer
  aucTXBuffer = (unsigned char*) realloc(aucTXBuffer, size_aucTXBuffer * sizeof(unsigned char)); //Sets aucTXBuffer size

  aucTXBuffer[0] = 0b0000000000110011;                                          //Header 1
  aucTXBuffer[1] = 0b0000000011001100;                                          //Header 2
  aucTXBuffer[2] = ((unsigned char)(uiEpoch_aucTXBuffer & 0b0000000011111111)); //Epoch
  aucTXBuffer[3] = ((unsigned char)(uiIdx_auiChA & 0b0000000011111111));        //Array size
  aucTXBuffer[4] = ((unsigned char)(bOverflow_auiChA & 0b0000000011111111));    //Array overflow?
  
  int idx_txbuf = size_header_TXBuffer;                                         //Index starts after header at aucTXBuffer
  
  for (int i = 0; i < uiIdx_auiChA; i++)
  {
    aucTXBuffer[idx_txbuf++] = ((unsigned char)
    ((auiChA[i] & 0b0000001111100000) >> 5) | 0b0000000011100000);              // MSB 111XXXXX
    aucTXBuffer[idx_txbuf++] = ((unsigned char)
    ((auiChA[i] & 0b0000000000011111)));                                        // LSB 000XXXXX
  }
  aucTXBuffer[idx_txbuf] = 0b0000000010011011;                                  // Foot

  uiEpoch_aucTXBuffer++;                                                        // Epoch Increment
  if (uiEpoch_aucTXBuffer >= 200) 
    uiEpoch_aucTXBuffer = 0;

  
  uiIdx_auiChA = 0;                                                             // Sets Idx auiCha to original state
  bOverflow_auiChA = false;                                                     // Sets bOverflow_auiChA to original state

  for (int i = 0; i < size_aucTXBuffer; i++)
  {
    Serial.write(aucTXBuffer[i]);
    Serial.flush();
  }
}

void Toggle_LED1()
{
  if (digitalRead(LED1) == HIGH) 
    digitalWrite(LED1, LOW);
  else 
    digitalWrite(LED1, HIGH);
}

unsigned int SignalProcess(unsigned int uiData)
{
  if(RECTIFY == 1)
    uiData = SignalRectifier(uiData);
    
  if(FILTER == 1)
    uiData = SignalFilter(uiData);

  return uiData;  
}

unsigned int SignalRectifier(unsigned int uiData)
{
  return uiData; 
}

unsigned int SignalFilter(unsigned int uiData)
{
  return uiData;
}

