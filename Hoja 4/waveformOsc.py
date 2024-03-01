#Ejercicio 1

from consts import *
import numpy as np         
import sounddevice as sd       
import matplotlib.pyplot as plt
import scipy.signal as signal

class WaveformOsc:
    def __init__(self,freq=440.0,amp=1.0,phase=0.0, waveform="Sine"):
        self.freq = freq
        self.amp = amp
        self.phase = phase
        self.frame = 0
        self.waveform = waveform

    def next(self):    
        arange = 2*np.pi*np.arange(self.frame,self.frame+CHUNK)*self.freq/SRATE
        out = None
        if self.waveform == "Sine":
            out = self.amp*np.sin(arange)
        elif self.waveform == "Square":
            out = self.amp * signal.square(arange)
        elif self.waveform == "Triangle":
            out = self.amp * signal.sawtooth(arange,0.5)
        elif self.waveform == "Sawtooth":
            out = self.amp * signal.sawtooth(arange)
        self.frame += CHUNK
        return out


class WaveformOscFM:
    def __init__(self,fc=110.0,amp=1.0,fm=6.0, beta=1.0, carrier="Sine", mod="Sine"):
        self.fc = fc
        self.amp = amp
        self.fm = fm
        self.beta = beta
        self.frame = 0

        # moduladora = βsin(2πfm)
        self.carrier = carrier
        self.mod = WaveformOsc(freq=fm,amp=beta,waveform=mod)
        
    def next(self):  
        # sin(2πfc+mod)  
        # sacamos el siguiente chunk de la moduladora
        mod = self.mod.next()

        # soporte para el chunk de salida
        sample = np.arange(self.frame,self.frame+CHUNK)  
        sample = 2*np.pi*self.fc*sample/SRATE +mod     
        # aplicamos formula
        if self.carrier == "Sine":
            out =  self.amp*np.sin(sample)
        elif self.carrier == "Square":
            out = self.amp*signal.square(sample)
        elif self.carrier == "Triangle":
            out = self.amp * signal.sawtooth(sample,0.5)
        elif self.carrier == "Sawtooth":
            out = self.amp * signal.sawtooth(sample)
        self.frame += CHUNK
        return out 
