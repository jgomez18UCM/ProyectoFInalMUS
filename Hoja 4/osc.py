
from consts import *
import numpy as np         
import sounddevice as sd       
import matplotlib.pyplot as plt
import scipy.signal as signal

class Osc:
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
