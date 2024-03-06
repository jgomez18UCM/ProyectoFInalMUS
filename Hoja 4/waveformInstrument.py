from instrument import *
from waveformSynthFM import *
from tkinter import * 
from tkinter import ttk

class WaveformInstrument(Instrument):
    def __init__(self,tk,name="FM synthetizer",amp=0.2,ratio=3,beta=0.6): 
        Instrument(tk,name,amp,ratio,beta)
        self.modCB = ttk.Combobox(name="Modulator",values=["Sine", "Square", "Sawtooth", "Triangle"])
        self.carrierCB = ttk.Combobox(name="Carrier",values=["Sine", "Square", "Sawtooth", "Triangle"])
        self.modCB.pack(side=TOP)
        self.carrierCB.pack(side=TOP)
    
    def noteOn(self,midiNote):
        # si está el dict de canales reactivamos synt -> reiniciamos adsr del synt
        if midiNote in self.channels:       
            self.channels[midiNote].start() 

        # si no está, miramos frecuencia en la tabla de frecuencias
        # y generamos un nuevo synth en un canal indexado con notaMidi
        # con los parámetros actuales del synth
        else:
            freq= freqsMidi[midiNote]
            self.channels[midiNote]= WaveformSynthFM(
                    fc=freq,
                    amp=self.ampS.get(), ratio=self.ratioS.get(), beta=self.betaS.get(),
                    attack = self.attackS.get(), decay= self.decayS.get(),
                    sustain=self.sustainS.get(), release=self.sustainS.get(),carrier=self.carrierCB.get(), mod=self.modCB.get())

def test():
