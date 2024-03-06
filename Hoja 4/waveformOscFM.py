from consts import *
import numpy as np         
import sounddevice as sd       
import matplotlib.pyplot as plt
import scipy.signal as signal
from waveformOsc import *
from oscFM import *

class WaveformOscFM(OscFM):
    def __init__(self,fc=110.0,amp=1.0,fm=6.0, beta=1.0, carrier="Sine", mod="Sine"):
        OscFM.__init__(self,fc,amp,fm,beta)
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

    def setWaveforms(self, carrier, mod):
        self.carrier = carrier
        self.mod.setWaveForm(mod)
