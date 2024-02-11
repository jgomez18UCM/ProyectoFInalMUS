#%%
import numpy as np         # arrays    
import sounddevice as sd   # modulo de conexión con portAudio
import soundfile as sf     # para lectura/escritura de wavs
CHUNK = 2048
SRATE = 4100

#%%
import pygame

class KBHit:
    def __init__(self) -> None:        
        pygame.init()
        pygame.key.set_repeat(100) # repeticion de pulsación 
        pygame.display.set_mode((300, 200)) # ventana de pygame
        pygame.display.set_caption('Keyboard input control')
        for event in pygame.event.get(): pass # limpiamos posible input previo
       
    def getKey(self):
        for event in pygame.event.get():
            if event.type == pygame.KEYDOWN: # si se pulsa tecla            
                if event.key == pygame.K_UP:    # cursores
                    return 'up'
                elif event.key == pygame.K_DOWN:
                    return 'down'
                elif event.key == pygame.K_LEFT:
                    return 'left'
                elif event.key == pygame.K_RIGHT:
                    return 'right'       
                elif event.key == pygame.K_ESCAPE: # escape
                    return 'escape'
                elif event.key == pygame.K_SPACE: # esapcio
                    return 'space'
                else:
                    return event.unicode # letras
            return ' '
        
    def quit(self):
        pygame.quit()


#%%

class Osc:
    def __init__(self, freq = 1, volume = 1, phase = 0):
        self.freq = freq
        self.volume = volume
        self.phase = phase
        self.frame = -0


    def next(self):
        chunk = np.arange(self.frame*CHUNK, (self.frame+1)*CHUNK, dtype=np.float32)
        self.frame+=1
        chunk = np.sin(self.phase + chunk*2*np.pi*self.freq/CHUNK)
        chunk*=self.volume
        return chunk

    def getVolume(self):
        return self.volume
    def setVolume(self, volume):
        self.volume = volume
        
    def getFrequency(self):
        return self.freq
    def setFrequency(self, freq):
        self.freq = freq

def testOsc():
    osc = Osc()
    input = KBHit()
    end = False
    stream = sd.OutputStream(samplerate=SRATE, blocksize=CHUNK, channels=1)
    stream.start()
    while not(end):
        chunk = osc.next()
        stream.write(chunk)
        key = input.getKey()
        if key != '':
            end = (key == 'escape')
            if key == 'v':
                osc.setVolume(osc.getVolume()-0.5)
            elif key == 'V':
                osc.setVolume(osc.getVolume()+0.5)
            elif key == 'f':
                osc.setFreq(osc.getFrequency()-0.5)
            elif key == 'F':
                osc.setFreq(osc.getFrequency()+0.5)
        
    input.quit()
    stream.stop()
    stream.close()

testOsc()