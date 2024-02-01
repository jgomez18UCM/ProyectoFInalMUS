##Jose Maria Gomez Pulido
##Eva Sánchez Muñoz
##Álvaro Velasco Díaz
#%%
import matplotlib.pyplot as plt
import numpy as np
import time

%matplotlib inline

SRATE = 4100

#%%
def noise1(dur):
    a = np.empty(SRATE*dur)
    for i in range(a.size):
        # convertimos valores de [0,1) en [-1,1)
        # a[i] = np.random.random()*2-1      

        # otra forma mád directa
        a[i] = np.random.uniform(-1,1)
    return a

def noise2(dur):
    # uniform necesita int
    return np.random.uniform(-1,1,round(dur*SRATE))

start = time.time()
dur = 10
a1 = noise1(dur)
print(f'time: {time.time() - start}')
start = time.time()
dur = 10
a2 = noise2(dur)
print(f'time: {time.time() - start}')

plt.plot(a1[:20])

plt.plot(a2[:20])


#%%
def osc(freq,dur=1,amp=1,phase=0):
    osc = np.arange(SRATE*dur)
    osc = amp*np.sin(phase+osc*2*np.pi*freq/SRATE)
    return osc

##2.1
s1 = osc(1)
plt.plot(s1)

#%% 2.2
s2 = osc(2)
plt.plot(s2)

#%% 2.3
s3 = osc(3,2)
plt.plot(s3)

#%% ejercicio 3
ej = osc(2,1,1,np.pi/2)
plt.plot(ej)

ej2 = osc(2,1,3,np.pi/2)
plt.plot(ej2)

#%%
def modulator(sample,freq):
    mod = sample*(osc(freq,amp=1/2)+0.5)
    return mod

ruido = noise2(1)
#Dibujamos el oscilador encima de la modulación en la gráfica, 
#Para comprobar que es correcto
plt.plot(osc(2,amp=1/2)+0.5)
mod = modulator(ruido, 2)

plt.plot(mod)

#%%
def nota_armonico(n,f,v):
    notas = [osc(x*f,amp=v/x) for x in range(1,n+1)]
    return np.array(notas)

notas = nota_armonico(4,1,1)
[plt.plot(nota) for nota in notas]

#%%
def square(freq, dur=1, amp=1, phase=0):
    sq = osc(freq,dur,amp,phase)
    sq = np.sign(sq)*amp
    return sq

plt.plot(square(1,amp=2,phase=10))

#%%
def triangulo(freq,dur=1,amp=1,phase=0):
    tr = osc(freq,dur,amp,phase)
    tr = 2*amp*np.arcsin(tr)/np.pi
    return tr

plt.plot(triangulo(1))

#%%
def saw(freq,dur=1,amp=1,phase=0):
    s = np.arange(SRATE*dur)
    s = phase+2*np.pi*s*freq/(2*SRATE)
    s = np.tan(s)
    s = 2*amp/np.pi*np.arctan(s)
    return s

plt.plot(saw(1,10))

#%%
def fadeOut(sample,t):
    t = t*SRATE
    fOut = np.arange(t)/t
    mSample = sample.copy()
    fOut = fOut[::-1]
    mSample[-t:] = mSample[-t:]*fOut
    return mSample

def fadeIn(sample,t):
    t=t*SRATE
    fIn = np.arange(t)/t
    mSample = sample.copy()
    mSample[:t] *= fIn
    return mSample

onda = osc(1,10)
#plt.plot(fadeOut(onda,2))
plt.plot(fadeIn(onda,2))

