# TLOU

## Descripción General

**TLOU** es un proyecto desarrollado en Unity que implementa sistemas avanzados de locomoción, inteligencia artificial (IA), gestión de entidades y materiales, y utilidades personalizadas, para toda la famila :D (o talvez no? ಠ_ಠ)

---

## Estructura del Proyecto

- **Assets/**  
  Carpeta principal de recursos del proyecto. Incluye:
  - `_test/`: Pruebas y experimentos.
  - `_utilities/`: Utilidades y scripts de apoyo.
  - `Entities/`: Prefabs y scripts relacionados con entidades del juego.
  - `Materials/`: Materiales usados en el proyecto.
  - `Plugins/`: Plugins externos, como el sistema de locomoción.
  - `Scenes/`: Escenas del juego.
  - Otros recursos y metadatos.

---

## Estructura y Funciones de AI

La carpeta `Assets/Entities/_Scripts/AI` organiza los sistemas principales de inteligencia artificial del proyecto. Sus componentes y responsabilidades son:

- **Aim/**
  - Gestiona el sistema de puntería de los enemigos, permitiendo que apunten y disparen con precisión al jugador u otros objetivos.
  - Ejemplo: `EnemyAimSystem.cs` controla la lógica de apuntado.

- **AlarmSystem/**
  - Implementa un sistema de alarmas para la comunicación entre enemigos.
  - `AlarmEmiter.cs`: Permite a un enemigo emitir una alarma que notifica a otros en un radio determinado.
  - `AlarmReceiver.cs`: Recibe y gestiona las alarmas, activando comportamientos de alerta en los enemigos.
  - `AlarmData.cs`: Define la información que se transmite con cada alarma.

- **Art/**
  - Contiene recursos gráficos (imágenes) que representan visualmente los distintos estados de la IA (por ejemplo, patrullando, buscando, combate, etc.).

- **Cover/**
  - Gestiona la lógica de coberturas en el entorno.
  - `CoversHandler.cs`: Organiza y proporciona puntos de cobertura para que los enemigos puedan protegerse estratégicamente durante el combate.

- **Detection/**
  - Encargada de la lógica de detección de objetivos, como el jugador, mediante visión, audición u otros sensores.

- **Faction/**
  - Permite organizar entidades en distintas facciones o equipos, facilitando la lógica de aliados y enemigos.

- **PushDownAutomata/**
  - Implementa el núcleo del sistema de estados de la IA usando un autómata de pila (Push Down Automata).
  - Permite a los enemigos cambiar de comportamiento (patrulla, persecución, combate, etc.) de forma flexible y jerárquica.

- **IAMovementController.cs**
  - Controla el movimiento general de los agentes de IA, integrando navegación, persecución y patrullaje.

---

## Controles del Jugador

El jugador utiliza los siguientes controles para moverse e interactuar:

| Tecla                | Acción                        |
|----------------------|------------------------------|
| **W**                | Avanzar                      |
| **S**                | Retroceder                   |
| **A**                | Moverse a la izquierda       |
| **D**                | Moverse a la derecha         |
| **Q**                | Modo escucha                 |
| **Ctrl Izquierdo**   | Agacharse                    |
| **Shift Izquierdo**  | Correr                       |

Estos controles están definidos en el script del jugador (por ejemplo, `PlayerController.cs`) y pueden ser modificados desde el Input Manager de Unity.

---

## Estructura y Funciones de la Carpeta Player

La carpeta `Assets/Entities/Player/Scripts` contiene los sistemas principales que gestionan el comportamiento y las capacidades del jugador. Sus componentes y responsabilidades son:

- **PlayerMovement.cs**
  - Controla el movimiento del jugador, incluyendo desplazamiento, velocidad y dirección según la cámara.
  - Gestiona la física y la interacción con el entorno.

- **PlayerBodyDirection.cs**
  - Sincroniza la dirección del cuerpo del jugador con la cámara para una orientación natural.

- **PlayerCameraHandler.cs**
  - Administra la cámara del jugador, incluyendo transiciones y efectos de combate (como sacudidas de cámara).

- **PlayerCombatController.cs**
  - Gestiona las acciones de combate cuerpo a cuerpo del jugador.

- **Weapon/**
  - **PlayerWeaponSystem.cs**: Sistema central de manejo de armas (equipar, disparar, recargar, etc.).
  - **PlayerWeaponInput.cs**: Captura la entrada del usuario para disparar y recargar armas.
  - **PlayerAimController.cs**: Controla el apuntado del jugador, alineando la mira con la cámara.
  - Otros scripts relacionados con el manejo y lógica de armas.

- **ListenMode/**
  - **ListenMode.cs**: Implementa el "modo escucha", permitiendo detectar enemigos cercanos y resaltarlos visualmente.
  - **ListenModeController.cs**: Gestiona la activación/desactivación del modo escucha mediante una tecla.
  - **ListenModeTarget.cs**: Componente que permite a los enemigos ser resaltados cuando el modo escucha está activo.

- **HeartRateSystem/**
  - **HeartRateController.cs**: Controla el ritmo cardíaco del jugador, afectando sonidos.
  - **MovementHeartRateMediator.cs**: Sincroniza el ritmo cardíaco con el movimiento del jugador.
  - **HeartRateStates/**: Define los distintos estados de ritmo cardíaco y sus sonidos asociados.

---

## Plugins y Sistemas Externos

### Locomotion System

- Ubicación: `Assets/Plugins/Locomotion System Files`
- Incluye scripts como `Util`, `DrawArea`, y `SmoothFollower`.
- Documentación: `documentation.html`
- Permite animaciones realistas de locomoción para personajes.

---

## Compilación y Ejecución

1. Abre el proyecto en Unity Hub.
2. Asegúrate de tener instaladas las dependencias necesarias (ver carpeta `Packages`).
3. Abre la solución `TLOU-TLOU.sln` en Visual Studio para editar scripts.
4. Ejecuta la escena principal desde el editor de Unity.

---

## Personalización y Extensión

- Puedes agregar nuevas escenas en la carpeta `Assets/Scenes`.
- Los materiales y entidades pueden ser gestionados en sus respectivas carpetas.
- Para modificar o extender el sistema de locomoción, revisa los scripts en `Assets/Plugins/Locomotion System Files/Locomotion System`.

---

## Créditos y Licencias

- El sistema de locomoción está licenciado por Rune Skovbo Johansen & Unity Technologies ApS. Ver el archivo "TERMS OF USE" en la carpeta del plugin para más detalles.

---


Espero que le puedan sacar probecho (づ｡◕‿‿◕｡)づ
