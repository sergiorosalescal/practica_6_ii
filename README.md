# Práctica 6 - Interacción en Realidad Virtual con XR Interaction Toolkit

**Autores:** 
1. **Adrián Pérez Poleo** (alu0101635224@ull.edu.es)
2. **Javier Acosta Portocarrero** (alu0101660769@ull.edu.es)
3. **Sergio Rosales Calzadilla** (alu0101635590@ull.edu.es)

## Índice

- [Introducción](#introducción)
- [Configuración del proyecto](#configuración-del-proyecto)
- [Configuración de la escena](#configuración-de-la-escena)
  - [XR Origin](#xr-origin)
  - [Interactuadores: Ray Interactor y Direct Interactor](#interactuadores-ray-interactor-y-direct-interactor)
  - [Interaction Layers](#interaction-layers)
  - [Interactuables](#interactuables)
  - [Interfaz de usuario](#interfaz-de-usuario)
- [Realización del Ejercicio](#realización-del-ejercicio)
  - [Cubo y esfera: Ray Interactor que cambia de color](#cubo-y-esfera-ray-interactor-que-cambia-de-color)
  - [Objeto de la Asset Store: Direct Interactor que actualiza la UI](#objeto-de-la-asset-store-direct-interactor-que-actualiza-la-ui)

## Introducción

Esta práctica de la asignatura **Interfaces Inteligentes** se centra en la interacción en realidad virtual mediante el **XR Interaction Toolkit** de Unity.

El toolkit separa la interacción en tres piezas: los **interactuadores** (*Interactors*), que son las manos o mandos del jugador y detectan los objetos con los que se puede interactuar; los **interactuables** (*Interactables*), que son los objetos de la escena que se pueden seleccionar o agarrar; y el **Interaction Manager**, que gestiona los eventos entre ambos. Sobre esta base se utilizan un **Ray Interactor**, que detecta objetos a distancia mediante un rayo, y un **Direct Interactor**, que detecta objetos por contacto mediante un collider. Todo el funcionamiento se prueba con el **simulador genérico del XR Interaction Toolkit**, sin necesidad de gafas.


## Configuración del proyecto

Se han instalado los paquetes esenciales (`XR Plugin Management`, `OpenXR`, `XR Interaction Toolkit` e `Input System`) y se han importado los **Starter Assets** y el **XR Interaction Simulator**. 

En los ajustes de OpenXR se configuraron los perfiles de los mandos de Meta/Oculus, y el simulador está ajustado para instanciarse automáticamente al entrar al modo Play en el editor.

## Configuración de la escena

### XR Origin

1. Se elimina la `Main Camera` de la escena, ya que el rig trae su propia cámara que sigue la cabeza. Con dos cámaras la vista podría salir mal.
2. Se añade el prefab **`XR Origin (XR Rig)`** de los *Starter Assets*. Es el cuerpo del jugador: define el suelo, la cámara (cabeza) y los dos mandos. Ya incluye la locomoción (`Locomotion Mediator`, movimiento continuo y `Teleportation Provider`).

### Interactuadores: Ray Interactor y Direct Interactor
Para separar la interacción con objetos cercanos y lejanos, se desactivó el `Near-Far Interactor` que viene por defecto en el mando derecho y se sustituyó por dos interactuadores independientes:

- **Ray Interactor**: Se le asignaron las acciones de *Select*, *Activate* y *UI Press*. Se amplió su alcance a 30 metros para llegar a los objetos del fondo y se ajustó la línea visual para que cambie de color al detectar un interactuable.
- **Direct Interactor**: Utiliza un `Sphere Collider` en modo *Trigger* para detectar los objetos cercanos que tocan la mano. Se mapeó con las acciones de *Select* y *Activate*.

### Interaction Layers

Para que el cubo y la esfera solo respondan al rayo y el objeto de la Asset Store solo a la mano, se usa la **Interaction Layer Mask**, que define qué capas incluye o excluye cada interactuador. Se crean dos capas, `Rayo` y `Directo`, y se asignan así:

| Objeto                                 | Componente             | Interaction Layer Mask |
|----------------------------------------|------------------------|------------------------|
| `Right Controller > Ray Interactor`    | XR Ray Interactor      | `Rayo`                 |
| `Right Controller > Direct Interactor` | XR Direct Interactor   | `Directo`              |
| Cubo                                   | XR Grab Interactable   | `Rayo`                 |
| Esfera                                 | XR Grab Interactable   | `Rayo`                 |
| Pistola (Asset Store)                  | XR Grab Interactable   | `Directo`              |

Un interactuador solo puede seleccionar un interactuable si ambos comparten al menos una capa. De este modo, aunque la mano toque el cubo o el rayo apunte a la pistola, la interacción no se produce.

### Interactuables

Los tres objetos tienen los componentes necesarios para ser interactuables:

| Componente             | Qué aporta                                                       |
|------------------------|------------------------------------------------------------------|
| `Collider`             | Permite que los interactuadores lo detecten (*Is Trigger* a falso) |
| `Rigidbody`            | Física: el objeto cae, rebota y se puede lanzar al soltarlo       |
| `XR Grab Interactable` | Lo marca como agarrable y define cómo se comporta al cogerlo      |

### Interfaz de usuario

Para mostrar mensajes dentro de la realidad virtual se utiliza un `Canvas` en modo **World Space**, colocado flotando delante del jugador, con un texto de TextMeshPro. En el `EventSystem` se sustituye el *Input System UI Input Module* por el **XR UI Input Module**, que es el que permite a los interactuadores trabajar con la interfaz.

---

## Realización del Ejercicio

> Agrega en la escena un cubo, una esfera y un objeto de la asset store que elijas sobre un plano, cada uno de ellos a diferentes distancias. El cubo y esfera serán Ray Interactor que cambian de color al seleccionarlo. El objeto de la asset store será un Direct Interactor que actualiza la UI al agarrarlo.

Los objetos están distribuidos así: la pistola (Asset Store) justo delante para alcanzarla con la mano, la esfera a 16 metros y el cubo a 20 metros. Toda la lógica funciona conectando métodos públicos a los **eventos predefinidos** del `XR Grab Interactable` desde el Inspector, sin usar colisiones por código.

Siguiendo el esquema general de la interacción, el interactuador detecta el objeto (el rayo lo intersecta o la mano lo toca), el usuario pulsa el botón del mando y el interactuable recibe un evento que desencadena la acción. Los scripts solo contienen métodos públicos, que se enlazan a esos eventos desde el Inspector arrastrando el propio objeto y seleccionando el método.

### Cubo y esfera: Ray Interactor que cambia de color

El script **`xr_cambiar_color.cs`** se añade al cubo y a la esfera:

- En `Start()` guarda la referencia a su `Renderer` con `GetComponent<Renderer>()`.
- El método público `cambiar_color()` muestra por consola el objeto seleccionado y asigna al material un color aleatorio con `Random.ColorHSV()`, igual que en las prácticas de colisiones.
- En el `XR Grab Interactable` de cada objeto, el evento **Select Entered** llama a `cambiar_color()`. Así, cada vez que el rayo selecciona el objeto, este cambia de color.

Como los dos objetos están en la capa `Rayo`, solo el Ray Interactor puede seleccionarlos.

Script: [Assets/Scripts/Practica6/xr_cambiar_color.cs](Assets/Scripts/Practica6/xr_cambiar_color.cs)

![](Gifs/funcionamiento_ray_interactor_cambiar_color.gif)

### Objeto de la Asset Store: Direct Interactor que actualiza la UI

El script **`xr_actualizar_ui_agarre.cs`** se añade a la pistola:

- Recibe desde el Inspector la referencia al texto de la interfaz (`TMP_Text`) y el nombre del objeto que se mostrará en los mensajes.
- En `Start()` muestra el mensaje inicial `"Busca la pistola"`.
- El método `al_agarrar()` incrementa el contador `veces_agarrado` y muestra `"Has cogido la pistola"` junto con el número de veces que se ha cogido.
- El método `al_soltar()` muestra `"Has soltado la pistola"`.
- Ambos utilizan la función privada `mostrar_mensaje()`, que escribe el mensaje en la consola con `Debug.Log()` y lo asigna al texto de la UI. Si no hay ningún texto asignado, solo lo muestra por consola.
- En el `XR Grab Interactable` de la pistola, el evento **Select Entered** llama a `al_agarrar()` y el evento **Select Exited** llama a `al_soltar()`.

Como la pistola está en la capa `Directo`, solo se puede coger con el Direct Interactor, es decir, acercando la mano hasta tocarla.

Script: [Assets/Scripts/Practica6/xr_actualizar_ui_agarre.cs](Assets/Scripts/Practica6/xr_actualizar_ui_agarre.cs)

![](Gifs/funcionamiento_direct_interactor_objeto_asset.gif)
