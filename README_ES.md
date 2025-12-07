# 🎛️ DJ Mixer

<div align="center">

[![Unity Version](https://img.shields.io/badge/Unity-2019.4%20LTS-black.svg?style=flat&logo=unity)](https://unity3d.com/get-unity/download)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20macOS%20%7C%20iOS%20%7C%20Android-lightgrey.svg)](https://github.com/username/dj-mixer)
[![Hecho con Unity](https://img.shields.io/badge/Hecho%20con-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)

</div>

<p align="justify">
Aplicación de mezcla musical en tiempo real desarrollada en Unity. Simula un controlador DJ profesional con dos decks, ecualización de 3 bandas, crossfader y sistema de scratch.
</p>
<br>

<img width="1463" height="814" alt="Interfaz Principal" src="https://github.com/user-attachments/assets/7167d61a-31ac-47de-a7c5-5b3d0337cf6a" />
<br>

<br>
<div align="center">

**[🇬🇧 English Version](README.md)**

</div>

---

## ✨ Características

### 🎚️ Control de Audio

<p align="justify">
• <strong>Dos decks independientes</strong> con reproducción simultánea<br>
• <strong>Control de pitch</strong> (±100%, rango 0.5x - 2.0x)<br>
• <strong>Ecualización paramétrica de 3 bandas</strong> por deck (LOW: 100Hz, MID: 1kHz, HIGH: 10kHz)<br>
• <strong>Crossfader profesional</strong> con 3 modos de curva (Linear, Equal Power, Logarithmic)<br>
• <strong>Sistema de scratch en tiempo real</strong> con cálculo angular<br>
• <strong>Latencia optimizada</strong> (~40ms, suficiente para práctica y educación)
</p>

### 🎨 Interfaz de Usuario

<p align="justify">
• <strong>Knobs circulares personalizados</strong> con feedback visual por color<br>
• <strong>Reset rápido</strong> con click simple (threshold de 5px)<br>
• <strong>Rotación visual de vinilos</strong> sincronizada con pitch<br>
• <strong>Display de carátulas</strong> de álbumes dinámico<br>
• <strong>Controles tipo hardware</strong> (Play, Pause, Next, Reset)
</p>

### 🔊 Efectos y Samples

<p align="justify">
• <strong>Pads de efectos</strong> (one-shots: air horn, sirena, etc.)<br>
• <strong>Instrument samples</strong> con hotkeys configurables<br>
• <strong>Routing independiente</strong> (no interrumpe la música principal)
</p>

---

## 🎯 Casos de Uso

<p align="justify">
• <strong>Educación musical:</strong> Aprender conceptos de mezcla DJ<br>
• <strong>Práctica de DJing:</strong> Entrenar técnicas sin hardware costoso<br>
• <strong>Prototipado de interfaces:</strong> Experimentar con UX de audio<br>
• <strong>Demos y presentaciones:</strong> Mostrar conceptos de procesamiento de audio
</p>

> ⚠️ **Nota**: <p align="justify">Este proyecto NO está diseñado para performance profesional en vivo debido a la latencia inherente de Unity (~40ms vs <5ms de DAWs profesionales).</p>

---

## 🚀 Instalación

### Requisitos del Sistema

**Mínimos:**

<p align="justify">
• Unity 2019.4 LTS o superior<br>
• Windows 10, macOS 10.13+, o Linux (Ubuntu 18.04+)<br>
• 4 GB RAM<br>
• 500 MB espacio en disco
</p>

**Recomendados:**

<p align="justify">
• Unity 2019.4 LTS<br>
• 8 GB RAM<br>
• Tarjeta de sonido dedicada
</p>

**Probado en:**

<p align="justify">
• Unity 2019.4.40f1 LTS
</p>

### Clonar el Repositorio

```bash
git clone https://github.com/cesarsm24/DJMixer.git
cd DJMixer
```

### Abrir en Unity

<p align="justify">
1. Abre <strong>Unity Hub</strong><br>
2. Click en <strong>Add</strong> → <strong>Add project from disk</strong><br>
3. Selecciona la carpeta <code>DJMixer</code><br>
4. Abre el proyecto con <strong>Unity 2019.4 LTS</strong>
</p>

### Escena Principal

<p align="justify">
La escena principal se encuentra en:
</p>

```
Assets/Project/Scenes/Main/DJMIXER.unity
```

---

## 📖 Uso

### Controles Básicos

#### Deck 1 & 2

<p align="justify">
• <strong>Play/Pause:</strong> Reproducir o pausar la pista actual<br>
• <strong>Reset:</strong> Reiniciar la pista desde el principio<br>
• <strong>Next:</strong> Avanzar a la siguiente pista<br>
• <strong>Pitch Slider:</strong> Ajustar velocidad (±8% típico DJ)
</p>

#### Ecualización

<p align="justify">
• <strong>Knobs LOW/MID/HIGH:</strong> Ajustar graves, medios y agudos (±30 dB)<br>
• <strong>Click en knob:</strong> Reset a 0 dB<br>
• <strong>Drag circular:</strong> Ajustar valor
</p>

#### Crossfader

<p align="justify">
• <strong>Izquierda (0%):</strong> Solo Deck 1<br>
• <strong>Centro (50%):</strong> Ambos decks al máximo<br>
• <strong>Derecha (100%):</strong> Solo Deck 2
</p>

#### Scratch

<p align="justify">
• <strong>Click y arrastra</strong> el vinilo para efecto scratch<br>
• Funciona en ambas direcciones
</p>

---

## 🏗️ Arquitectura del Proyecto

### Estructura de Carpetas

```
Assets/Project/
├── Audio/
│   ├── Music/          # 6 pistas musicales
│   ├── SFX/            # 6 efectos de sonido
│   └── Instruments/    # 6 samples de instrumentos
├── AudioMixer/
│   └── Discos.mixer    # Configuración DSP
├── Scripts/
│   ├── Audio/          # ControlDisco, Crossfader
│   ├── Effects/        # Effects, Instruments
│   ├── UI/             # CircleSlider, RadialSlider
│   └── Visual/         # RotateDisk
├── Graphics/
│   ├── Textures/       # Knobs, botones, vinilos
│   └── AlbumCovers/    # Carátulas de álbumes
└── Scenes/
    └── Main/           # Escena principal
```

### Scripts Principales

<div align="center">

| Script | Líneas | Función |
|:-------|:------:|:--------|
| `ControlDisco.cs` | ~350 | Gestión de clips y reproducción |
| `Crossfader.cs` | ~200 | Mezcla entre decks |
| `CircleSlider.cs` | ~200 | Lógica de knobs circulares |
| `Instruments.cs` | ~250 | Pads de samples/loops |
| `RotateDisk.cs` | ~180 | Rotación visual y scratch |
| `Effects.cs` | ~150 | Efectos one-shot |
| `RadialSlider.cs` | ~100 | Detección de input |

</div>

<p align="justify">
<strong>Total:</strong> ~1,430 líneas, 100% documentadas con XML
</p>

### AudioMixer

```
Master
├── Disco_01
│   ├── Pitch Shifter (FFT 1024)
│   ├── ParamEQ Low (100 Hz)
│   ├── ParamEQ Mid (1000 Hz)
│   └── ParamEQ High (10000 Hz)
├── Disco_02
│   └── [misma configuración]
├── Effects
└── Instruments
```

<p align="justify">
<strong>Parámetros expuestos:</strong> 10 (pitch, volumen, EQ × 6)
</p>

---

## 🛠️ Desarrollo

### Patrón de Diseño

<p align="justify">
El proyecto utiliza <strong>Component-Based Architecture</strong>:
</p>

<p align="justify">
• Separación clara de responsabilidades<br>
• Bajo acoplamiento entre módulos<br>
• Alta cohesión dentro de cada script<br>
• Reutilización de componentes
</p>

### Flujo de Audio

```
Usuario → UI Scripts → Audio Scripts → AudioMixer → Speakers
```

<p align="justify">
<strong>Ejemplo: Ajustar EQ Low</strong>
</p>

<p align="justify">
1. Usuario arrastra knob<br>
2. <code>RadialSlider</code> detecta drag (>5px)<br>
3. <code>CircleSlider</code> calcula ángulo → dB<br>
4. <code>ControlDisco.SetFloat("Disco_01_Low", valor)</code><br>
5. <code>AudioMixer</code> aplica filtro ParamEQ<br>
6. Audio modificado en tiempo real
</p>

### Añadir Nuevas Pistas

<p align="justify">
1. Añade archivos <code>.ogg</code> a <code>Assets/Project/Audio/Music/</code><br>
2. Añade carátulas a <code>Assets/Project/Graphics/AlbumCovers/</code><br>
3. En Unity, selecciona <code>Disco_01</code> o <code>Disco_02</code><br>
4. En el componente <code>ControlDisco</code>:
</p>

<p align="justify">
&nbsp;&nbsp;&nbsp;• Añade clips al array <code>Clips</code><br>
&nbsp;&nbsp;&nbsp;• Añade sprites al array <code>Sprites</code>
</p>

---

## 🗺️ Roadmap

### v1.0 (Actual)

<p align="justify">
✅ Dos decks funcionales<br>
✅ Pitch control<br>
✅ EQ de 3 bandas<br>
✅ Crossfader profesional<br>
✅ Sistema de scratch<br>
✅ Efectos y samples
</p>

### v2.0 (Futuro)

<p align="justify">
🔮 Waveform display visual<br>
🔮 BPM detection automático<br>
🔮 Cue points y loops<br>
🔮 Más efectos (Reverb, Delay, Flanger)<br>
🔮 Key detection (Rueda de Camelot)
</p>

---

## Guías de Estilo

<p align="justify">
• <strong>Código C#:</strong> Seguir convenciones de Unity<br>
• <strong>Documentación:</strong> XML docs en todos los métodos públicos<br>
• <strong>Commits:</strong> Usar <a href="https://www.conventionalcommits.org/">Conventional Commits</a>
</p>

---

## 📚 Recursos

### Documentación

<p align="justify">
• <a href="https://docs.unity3d.com/Manual/AudioMixer.html">Unity Audio Mixer</a><br>
• <a href="https://www.dspguide.com/">DSP Guide</a><br>
• <a href="https://www.digitaldjtips.com/beatmatching/">Tutorial de Beatmatching</a>
</p>

### Inspiración

<p align="justify">
• <a href="https://www.native-instruments.com/en/products/traktor/">Traktor Pro</a><br>
• <a href="https://www.pioneerdj.com/">Pioneer DJ</a><br>
• <a href="https://www.virtualdj.com/">VirtualDJ</a>
</p>

---

## 📄 Licencia

<p align="justify">
Este proyecto está bajo la Licencia MIT. Ver el archivo <a href="LICENSE">LICENSE</a> para más detalles.
</p>

---

## 👤 Autor

**César Sánchez Montes**

<p align="justify">
• GitHub: <a href="https://github.com/cesarsm24">@cesarsm24</a><br>
• Email: csanchezop@alumnos.unex.es
</p>

---

## 🙏 Agradecimientos

<p align="justify">
• Assets de audio: <a href="https://freesound.org/">Freesound.org</a><br>
• Sprites de UI: <a href="https://www.freepik.com/">Freepik</a><br>
• Tipografía: Request Font<br>
• Inspiración: Comunidad de DJs y productores
</p>

---

## 📸 Screenshots

### Interfaz Inicio

<img width="1462" height="817" alt="Interfaz Entrada" src="https://github.com/user-attachments/assets/bba948f9-27df-4356-bd42-a0ee08a309d4" />

### Interfaz Principal

<img width="1463" height="814" alt="Interfaz Principal" src="https://github.com/user-attachments/assets/73e17135-b62a-4a23-ac7d-c5833527b679" />

### Panel de Efectos e Instrumentos

<img width="1449" height="827" alt="Panel Efectos" src="https://github.com/user-attachments/assets/e5dcd11b-5ab9-4bd5-8ed3-55b71357dc21" />

---

## ⭐ Soporte

<p align="justify">
Si este proyecto te ha sido útil, considera darle una estrella ⭐ en GitHub.
</p>

<p align="justify">
Para reportar bugs o sugerir features, abre un <a href="https://github.com/cesarsm24/DJMixer/issues">Issue</a>.
</p>

---

<p align="center">
  Hecho con ❤️ y Unity
</p>
