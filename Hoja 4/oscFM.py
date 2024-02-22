
import numpy as np   
import osc
import matplotlib.pyplot as plt
from consts import *
import scipy.signal as signal

class OscFM:
    def __init__(self,fc=110.0,amp=1.0,fm=6.0, beta=1.0, carrier="Sine", mod="Sine"):
        self.fc = fc
        self.amp = amp
        self.fm = fm
        self.beta = beta
        self.frame = 0

        # moduladora = βsin(2πfm)
        self.carrier = carrier
        self.mod = osc.Osc(freq=fm,amp=beta,waveform=mod)
        
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
