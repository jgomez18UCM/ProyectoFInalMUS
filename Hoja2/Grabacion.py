## Este programa no he conseguido hacerlo en un notebook, porque no me salia nada para meter el input por consola
import numpy as np
import sounddevice as sd
import soundfile as sf
from kbhit import KBHit

SRATE = 4800
CHUNK = 1024

class Osc:
    def __init__(self, freq = 440, volume = 1, phase = 0):
        self.freq = freq
        self.volume = volume
        self.phase = phase
        self.frame = -0


    def next(self):
        bloque = np.float32(np.arange(self.frame, self.frame+CHUNK))
        self.frame+=CHUNK
        bloque = np.sin(self.phase + bloque*2*np.pi*self.freq/SRATE)
        bloque*=self.volume
        return bloque

    def getVolume(self):
        return self.volume
    def setVolume(self, volume):
        self.volume = volume
        if self.volume < 0:
            self.volume = 0
        elif self.volume > 1:
            self.volume = 1
        
    def getFrequency(self):
        return self.freq
    def setFrequency(self, freq):
        self.freq = freq

stream = sd.InputStream(samplerate=SRATE, blocksize=CHUNK, dtype=np.float32, channels=1)
stream.start()
# buffer para grabación.
# (0,1): vacio (tamaño 0), 1 canal
buffer = np.empty((0, 1), dtype="float32")
kb = KBHit()
c = ''
# bucle de grabación
while c != 'escape':
 # recogida de samples en array
 bloque, _check = stream.read(CHUNK) # devuelve un par (samples,bool)
 buffer = np.append(buffer,bloque) # en bloque[0] están los samples
 c = kb.getKey()
stream.stop()
kb.quit()
# reproducción del buffer adquirido
c = input('Quieres reproducir [S/n]? ')
if c!='n':
 sd.play(buffer, SRATE)
 sd.wait()
# volcado a un archivo wav, utilizando la librería soundfile
c = input('Grabar a archivo [S/n**? ')
if c!='n':
 sf.write("rec.wav", buffer, SRATE)