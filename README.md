# 🎛️ DJ Mixer

<div align="center">

[![Unity Version](https://img.shields.io/badge/Unity-2019.4%20LTS-black.svg?style=flat&logo=unity)](https://unity3d.com/get-unity/download)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20macOS%20%7C%20iOS%20%7C%20Android-lightgrey.svg)](https://github.com/username/dj-mixer)
[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)

</div>

<p align="justify">
Real-time music mixing application developed in Unity. Simulates a professional DJ controller with two decks, 3-band equalization, crossfader, and scratch system.
</p>
<br>

<img width="1463" height="814" alt="Main Interface" src="https://github.com/user-attachments/assets/7167d61a-31ac-47de-a7c5-5b3d0337cf6a" /> <br>
<div align="center">

**[🇪🇸 Spanish Version](README_ES.md)**

</div>

---

## ✨ Features

### 🎚️ Audio Control

<p align="justify">
• <strong>Two independent decks</strong> with simultaneous playback<br>
• <strong>Pitch control</strong> (±100%, range 0.5x - 2.0x)<br>
• <strong>3-band parametric equalization</strong> per deck (LOW: 100Hz, MID: 1kHz, HIGH: 10kHz)<br>
• <strong>Professional crossfader</strong> with 3 curve modes (Linear, Equal Power, Logarithmic)<br>
• <strong>Real-time scratch system</strong> with angular calculation<br>
• <strong>Optimized latency</strong> (~40ms, sufficient for practice and education)
</p>

### 🎨 User Interface

<p align="justify">
• <strong>Custom circular knobs</strong> with color visual feedback<br>
• <strong>Quick reset</strong> with simple click (5px threshold)<br>
• <strong>Visual vinyl rotation</strong> synchronized with pitch<br>
• <strong>Dynamic album cover display</strong><br>
• <strong>Hardware-style controls</strong> (Play, Pause, Next, Reset)
</p>

### 🔊 Effects and Samples

<p align="justify">
• <strong>Effect pads</strong> (one-shots: air horn, siren, etc.)<br>
• <strong>Instrument samples</strong> with configurable hotkeys<br>
• <strong>Independent routing</strong> (doesn't interrupt main music)
</p>

---

## 🎯 Use Cases

<p align="justify">
• <strong>Music education:</strong> Learn DJ mixing concepts<br>
• <strong>DJ practice:</strong> Train techniques without expensive hardware<br>
• <strong>Interface prototyping:</strong> Experiment with audio UX<br>
• <strong>Demos and presentations:</strong> Showcase audio processing concepts
</p>

> ⚠️ **Note**: <p align="justify">This project is NOT designed for professional live performance due to Unity's inherent latency (~40ms vs <5ms of professional DAWs).</p>

---

## 🚀 Installation

### System Requirements

**Minimum:**

<p align="justify">
• Unity 2019.4 LTS or higher<br>
• Windows 10, macOS 10.13+, or Linux (Ubuntu 18.04+)<br>
• 4 GB RAM<br>
• 500 MB disk space
</p>

**Recommended:**

<p align="justify">
• Unity 2019.4 LTS<br>
• 8 GB RAM<br>
• Dedicated sound card
</p>

**Tested on:**

<p align="justify">
• Unity 2019.4.40f1 LTS
</p>

### Clone the Repository

```bash
git clone https://github.com/cesarsm24/DJMixer.git
cd DJMixer
```

### Open in Unity

<p align="justify">
1. Open <strong>Unity Hub</strong><br>
2. Click on <strong>Add</strong> → <strong>Add project from disk</strong><br>
3. Select the <code>DJMixer</code> folder<br>
4. Open the project with <strong>Unity 2019.4 LTS</strong>
</p>

### Main Scene

<p align="justify">
The main scene is located at:
</p>

```
Assets/Project/Scenes/Main/DJMIXER.unity
```

---

## 📖 Usage

### Basic Controls

#### Deck 1 & 2

<p align="justify">
• <strong>Play/Pause:</strong> Play or pause the current track<br>
• <strong>Reset:</strong> Restart the track from the beginning<br>
• <strong>Next:</strong> Advance to the next track<br>
• <strong>Pitch Slider:</strong> Adjust speed (±8% typical DJ)
</p>

#### Equalization

<p align="justify">
• <strong>LOW/MID/HIGH Knobs:</strong> Adjust bass, mids, and treble (±30 dB)<br>
• <strong>Click on knob:</strong> Reset to 0 dB<br>
• <strong>Circular drag:</strong> Adjust value
</p>

#### Crossfader

<p align="justify">
• <strong>Left (0%):</strong> Deck 1 only<br>
• <strong>Center (50%):</strong> Both decks at maximum<br>
• <strong>Right (100%):</strong> Deck 2 only
</p>

#### Scratch

<p align="justify">
• <strong>Click and drag</strong> the vinyl for scratch effect<br>
• Works in both directions
</p>

---

## 🏗️ Project Architecture

### Folder Structure

```
Assets/Project/
├── Audio/
│   ├── Music/          # 6 music tracks
│   ├── SFX/            # 6 sound effects
│   └── Instruments/    # 6 instrument samples
├── AudioMixer/
│   └── Discos.mixer    # DSP configuration
├── Scripts/
│   ├── Audio/          # ControlDisco, Crossfader
│   ├── Effects/        # Effects, Instruments
│   ├── UI/             # CircleSlider, RadialSlider
│   └── Visual/         # RotateDisk
├── Graphics/
│   ├── Textures/       # Knobs, buttons, vinyls
│   └── AlbumCovers/    # Album covers
└── Scenes/
    └── Main/           # Main scene
```

### Main Scripts

<div align="center">

| Script | Lines | Function |
|:-------|:-----:|:---------|
| `ControlDisco.cs` | ~350 | Clip management and playback |
| `Crossfader.cs` | ~200 | Mixing between decks |
| `CircleSlider.cs` | ~200 | Circular knobs logic |
| `Instruments.cs` | ~250 | Sample/loop pads |
| `RotateDisk.cs` | ~180 | Visual rotation and scratch |
| `Effects.cs` | ~150 | One-shot effects |
| `RadialSlider.cs` | ~100 | Input detection |

</div>

<p align="justify">
<strong>Total:</strong> ~1,430 lines, 100% documented with XML
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
│   └── [same configuration]
├── Effects
└── Instruments
```

<p align="justify">
<strong>Exposed parameters:</strong> 10 (pitch, volume, EQ × 6)
</p>

---

## 🛠️ Development

### Design Pattern

<p align="justify">
The project uses <strong>Component-Based Architecture</strong>:
</p>

<p align="justify">
• Clear separation of responsibilities<br>
• Low coupling between modules<br>
• High cohesion within each script<br>
• Component reusability
</p>

### Audio Flow

```
User → UI Scripts → Audio Scripts → AudioMixer → Speakers
```

<p align="justify">
<strong>Example: Adjusting EQ Low</strong>
</p>

<p align="justify">
1. User drags knob<br>
2. <code>RadialSlider</code> detects drag (>5px)<br>
3. <code>CircleSlider</code> calculates angle → dB<br>
4. <code>ControlDisco.SetFloat("Disco_01_Low", value)</code><br>
5. <code>AudioMixer</code> applies ParamEQ filter<br>
6. Audio modified in real-time
</p>

### Adding New Tracks

<p align="justify">
1. Add <code>.ogg</code> files to <code>Assets/Project/Audio/Music/</code><br>
2. Add cover art to <code>Assets/Project/Graphics/AlbumCovers/</code><br>
3. In Unity, select <code>Disco_01</code> or <code>Disco_02</code><br>
4. In the <code>ControlDisco</code> component:
</p>

<p align="justify">
&nbsp;&nbsp;&nbsp;• Add clips to the <code>Clips</code> array<br>
&nbsp;&nbsp;&nbsp;• Add sprites to the <code>Sprites</code> array
</p>

---

## 🗺️ Roadmap

### v1.0 (Current)

<p align="justify">
✅ Two functional decks<br>
✅ Pitch control<br>
✅ 3-band EQ<br>
✅ Professional crossfader<br>
✅ Scratch system<br>
✅ Effects and samples
</p>

### v2.0 (Future)

<p align="justify">
🔮 Visual waveform display<br>
🔮 Automatic BPM detection<br>
🔮 Cue points and loops<br>
🔮 More effects (Reverb, Delay, Flanger)<br>
🔮 Key detection (Camelot Wheel)
</p>

---

## Style Guidelines

<p align="justify">
• <strong>C# Code:</strong> Follow Unity conventions<br>
• <strong>Documentation:</strong> XML docs on all public methods<br>
• <strong>Commits:</strong> Use <a href="https://www.conventionalcommits.org/">Conventional Commits</a>
</p>

---

## 📚 Resources

### Documentation

<p align="justify">
• <a href="https://docs.unity3d.com/Manual/AudioMixer.html">Unity Audio Mixer</a><br>
• <a href="https://www.dspguide.com/">DSP Guide</a><br>
• <a href="https://www.digitaldjtips.com/beatmatching/">Beatmatching Tutorial</a>
</p>

### Inspiration

<p align="justify">
• <a href="https://www.native-instruments.com/en/products/traktor/">Traktor Pro</a><br>
• <a href="https://www.pioneerdj.com/">Pioneer DJ</a><br>
• <a href="https://www.virtualdj.com/">VirtualDJ</a>
</p>

---

## 📄 License

<p align="justify">
This project is licensed under the MIT License. See the <a href="LICENSE">LICENSE</a> file for more details.
</p>

---

## 👤 Author

**César Sánchez Montes**

<p align="justify">
• GitHub: <a href="https://github.com/cesarsm24">@cesarsm24</a><br>
• Email: csanchezop@alumnos.unex.es
</p>

---

## 🙏 Acknowledgments

<p align="justify">
• Audio assets: <a href="https://freesound.org/">Freesound.org</a><br>
• UI sprites: <a href="https://www.freepik.com/">Freepik</a><br>
• Typography: Request Font<br>
• Inspiration: DJ and producer community
</p>

---

## 📸 Screenshots

### Startup Interface

<img width="1462" height="817" alt="Startup Interface" src="https://github.com/user-attachments/assets/bba948f9-27df-4356-bd42-a0ee08a309d4" />

### Main Interface

<img width="1463" height="814" alt="Main Interface" src="https://github.com/user-attachments/assets/73e17135-b62a-4a23-ac7d-c5833527b679" />

### Effects and Instruments Panel

<img width="1449" height="827" alt="Effects Panel" src="https://github.com/user-attachments/assets/e5dcd11b-5ab9-4bd5-8ed3-55b71357dc21" />

---

## ⭐ Support

<p align="justify">
If this project has been useful to you, consider giving it a star ⭐ on GitHub.
</p>

<p align="justify">
To report bugs or suggest features, open an <a href="https://github.com/cesarsm24/DJMixer/issues">Issue</a>.
</p>

---

<p align="center">
  Made with ❤️ and Unity
</p>
