# JOSE MARIA GOMEZ PULIDO    
# EVA SANCHEZ MUÑOZ
# ALVARO VELASCO DIAZ

from consts import *
from tkinter import *
from slider import *
import sounddevice as sd
from instrument import *
import os

class PadInC(Instrument):
    def __init__(self,tk,name="FM synthetizer PadInC",amp=0.2,ratio=3,beta=0.6):
        Instrument.__init__(self, tk, name, amp, ratio, beta)

    def down(self,event):
        c = event.keysym

        # tecla "panic" -> apagamos todos los sintes de golpe!
        if c=='0': 
            self.stop()            
        elif c in teclas:
            midiNote = 48+teclas.index(c) # buscamos indice y trasnportamos a C3 (48 en midi)
            self.noteOn(midiNote)         # arrancamos noteOn con el instrumento 
            print(f'noteOn {midiNote}')
            if c not in "r5t6y7u":        #A partir de estas notas ya no existe la quinta justa
                if c in "sxdchnjm2w3e":       #En estas notas hay que sumar 3 semitonos para que sea tercera y no cuarta
                    midiNoteTercera = 48+teclas.index(c)+3
                    midiNoteQuinta = 48+teclas.index(c)+7
                if c == "m":              #La nota B
                    midiNoteQuinta = 48+teclas.index(c)+8
                else:
                    midiNoteTercera = 48+teclas.index(c)+4
                    midiNoteQuinta = 48+teclas.index(c)+7
                self.noteOn(midiNoteTercera)                #noteOn con la tercera
                self.noteOn(midiNoteQuinta)                 #noteOn con la quinta
                print(f'noteOn {midiNoteTercera}')
                print(f'noteOn {midiNoteQuinta}')

    def up(self,event):
        c = event.keysym
        if c in teclas:
            midiNote = 48+teclas.index(c) # buscamos indice y hacemos el noteOff
            self.noteOff(midiNote)
            if c not in "r5t6y7u":        #A partir de estas notas ya no existe la quinta
                if c in "sxdchnjm":       #En estas notas hay que sumar 3 semitonos para que sea tercera y no cuarta
                    midiNoteTercera = 48 + teclas.index(c)+3
                    midiNoteQuinta = 48+teclas.index(c)+7
                if c == "m":              #La nota B
                    midiNoteQuinta = 48+teclas.index(c)+8
                else:
                    midiNoteTercera = 48 + teclas.index(c)+4
                    midiNoteQuinta = 48+teclas.index(c)+7
                self.noteOff(midiNoteTercera)
                self.noteOff(midiNoteQuinta)

def test():
    def callback(outdata, frames, time, status):    
        if status: print(status)    
        s = np.sum([i.next() for i in inputs],axis=0)
        s = np.float32(s)
        outdata[:] = s.reshape(-1, 1)

    tk = Tk()
    ins = PadInC(tk)
    inputs = [ins]

    # desactivar repeticion de teclas
    os.system('xset r off')

    stream = sd.OutputStream(samplerate=SRATE, channels=1, blocksize=CHUNK, callback=callback)    
    stream.start()
    tk.mainloop()

    # reactivar repeticion de teclas   
    os.system('xset r on')
    stream.close()

test()    