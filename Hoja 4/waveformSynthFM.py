import numpy as np   
import matplotlib.pyplot as plt
from consts import *
from tkinter import *
from adsr import *
from waveformOscFM import *
from synthFM import *
class WaveformSynthFM(SynthFM):
    def __init__(self,
                fc=110,amp=1.0,ratio=0.5, beta=5.0,   # parámetros del generador FM
                attack=0.01,decay=0.02, sustain=0.3,release=1.0, carrier="Sine", mod = "Sine"): # parámetros del ADSR   
        SynthFM.__init__(self,fc,amp,ratio,beta,attack,decay,sustain,release)     
        self.signal = WaveformOscFM(self.fc,self.amp,self.fm,self.beta,carrier,mod) # generador
        

    def start(self):
        self.adsr.start()

    # siguiente chunk del generador
    def next(self): 
        out = self.signal.next()*self.adsr.next()
        if self.adsr.state=='off': # cuando acaba el adsr por completo (incluido el release)
            self.state = 'off'     # el sinte tb acaba de producir señal
        return out     
    
    # el noteOff del sinte activa el release del ADSR
    def noteOff(self):
        self.adsr.release()

    def setAmp(self,val): 
        self.amp = val 

    def setFm(self,val): 
        self.fm = val  

    def setBeta(self,val): 
        self.beta = val

    def setFMWaveform(self,carrier,mod):
        self.signal.setWaveforms(carrier,mod)
