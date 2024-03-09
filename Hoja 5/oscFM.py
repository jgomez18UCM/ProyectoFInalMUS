
import numpy as np   
from osc import *
import matplotlib.pyplot as plt
from consts import *
import scipy.signal as signal

class OscFM:
    def __init__(self,fc=110.0,amp=1.0,fm=6.0, beta=1.0):
        self.fc = fc
        self.amp = amp
        self.fm = fm
        self.beta = beta
        self.frame = 0

       
        self.mod = Osc(freq=fm,amp=beta)
        
    def next(self):  
        # sin(2πfc+mod)  
        # sacamos el siguiente chunk de la moduladora
        mod = self.mod.next()

        # soporte para el chunk de salida
        bloque = np.float32(np.arange(self.frame, self.frame+CHUNK))
        bloque = self.amp*np.sin(bloque*2*np.pi*self.fc/SRATE+mod)
        self.frame += CHUNK
        return bloque
