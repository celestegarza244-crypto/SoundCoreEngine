# SoundCore Engine v2.0

Controlador de Set DJ mediante Lista Enlazada Simple Personalizada
Reto de la Unidad 2 · Estructuras de Datos · TecNM Campus Monclova

---

👥Autoras

- 👤Alexa Abigail Fraire Sandoval
- 👤Celeste Anyelique Garza Mauricio

---

## 1. Descripción general

SoundCore Engine es una aplicación de escritorio desarrollada en C# 14 sobre .NET 10 con Windows Forms. Simula el controlador de un set de DJ: permite registrar canciones con sus metadatos (título, artista, BPM y duración) y administrarlas en una cola de reproducción en vivo.

El objetivo académico es implementar desde cero una lista enlazada simple genérica (`SinglyLinkedList<T>`), manipulando directamente referencias en memoria, y contrastar su comportamiento contra las colecciones nativas de .NET (`LinkedList<T>` y `List<T>`) mediante una prueba de rendimiento con `Stopwatch`.

Además de los requisitos del reto, el proyecto incluye dos módulos adicionales: un reproductor de audio (G Player) y un módulo de sincronización en red entre varias computadoras.

## 2. Requisitos

- Sistema operativo Windows (Windows Forms solo compila y se ejecuta en Windows).
- .NET 10 SDK con la carga de trabajo "Desarrollo de escritorio de .NET", o **Visual Studio 2022 (17.10 o superior).
- Windows Media Player habilitado (solo para G Player).
- No se utilizan paquetes NuGet externos.

## 3. Cómo ejecutar

Desde Visual Studio: abrir `SoundCoreEngine.csproj` y presionar F5.

Desde consola, dentro de la carpeta del proyecto:

```
dotnet build
dotnet run
```

Al iniciar se abren tres ventanas: el controlador principal, el reproductor G Player y la ventana de Red.

## 4. Estructura del proyecto

```
SoundCoreEngine/
├── Models/
│   └── Track.cs                  Registro inmutable con los datos de la canción
├── CustomStructures/
│   ├── Node.cs                   Nodo genérico autorreferenciado
│   ├── IPlaybackQueue.cs         Interfaz común para las tres colecciones
│   ├── SinglyLinkedList.cs       Lista enlazada simple con los 6 algoritmos
│   ├── LinkedListAdapter.cs      Adaptador de LinkedList<T> nativa
│   └── ListAdapter.cs            Adaptador de List<T> nativa
├── UI/
│   ├── MainForm.cs               Lógica de la ventana principal
│   ├── MainForm.Designer.cs      Diseño de controles (tema oscuro)
│   └── PlayerForm.cs             Reproductor G Player
├── Networking/
│   ├── NetworkSync.cs            Sincronización TCP entre equipos
│   └── NetworkForm.cs            Ventana de conexión en red
├── Program.cs                    Punto de entrada
├── SoundCoreEngine.csproj        Configuración del proyecto
├── README.md                     Este documento
└── NETWORK.md                    Guía breve del módulo de red
```

El diseño sigue una **arquitectura por capas**: la estructura de datos (`CustomStructures`) no conoce nada de Windows Forms, por lo que es completamente reutilizable (desacoplamiento). La interfaz solo trabaja contra `IPlaybackQueue<Track>`, sin saber qué implementación concreta está activa.

## 5. Descripción detallada de cada componente

### 5.1 Modelo: `Track`

Es un `record` inmutable con cinco propiedades: `Id`, `Title`, `Artist`, `Bpm` y `DurationSeconds`. Al ser un record, dos canciones con los mismos valores se consideran iguales, lo que se aprovecha en la sincronización de red. Su método `ToString()` produce el formato `[001] Título - Artista | 120 BPM`.

### 5.2 Nodo: `Node<T>`

Contiene el valor almacenado (`Value`) y una referencia al siguiente nodo (`Next`). No depende de ninguna colección externa.

### 5.3 Interfaz: `IPlaybackQueue<T>`

Contrato común que permite intercambiar la implementación en tiempo de ejecución. Define `Count`, `IsEmpty`, `AddToEnd`, `PlayNext`, `AdvanceTrack`, `Reverse`, `InsertSorted`, `RemoveDuplicates` y `Clear`, y hereda de `IEnumerable<T>` para poder enlazarse a la tabla de la interfaz.

### 5.4 Lista enlazada simple: `SinglyLinkedList<T>`

Mantiene un puntero a la cabeza (`_head`), un puntero persistente a la cola (`_tail`) y un contador. No usa arreglos ni colecciones auxiliares. Implementa seis algoritmos:

| Método | Complejidad | Descripción |
|---|---|---|
| `AddToEnd(T)` | O(1) | Agrega al final usando el puntero de cola. |
| `PlayNext(T)` | O(1) | Inserta justo después de la cabeza ("Up Next VIP"). |
| `AdvanceTrack()` | O(1) | Extrae la cabeza; lanza `InvalidOperationException` si la cola está vacía. |
| `Reverse()` | O(n) tiempo, O(1) espacio | Inversión in situ con la técnica de tres punteros (`previous`, `current`, `next`). No crea listas temporales ni intercambia valores; reorienta referencias. |
| `InsertSorted(T, cmp)` | O(n) | Inserta manteniendo el orden según un comparador (por ejemplo, BPM). |
| `RemoveDuplicates(eq)` | O(n²) | Elimina repetidos con dos punteros anidados, conservando la primera aparición y sin usar `HashSet` ni arreglos. |

Los casos límite (lista vacía, un solo elemento, actualización de cabeza y cola) están contemplados en cada método. La clase implementa `IEnumerable<T>` con `yield return`.

### 5.5 Adaptadores nativos

• `LinkedListAdapter<T>`: envuelve `LinkedList<T>` (nodos doblemente enlazados). La inserción a mitad de lista es O(1). Como .NET no ofrece `Reverse()` en esta clase, la inversión se hace reconstruyendo la lista desde el último nodo.
- `ListAdapter<T>`: envuelve `List<T>` (arreglo dinámico contiguo). Se incluye intencionalmente como el caso costoso del benchmark: insertar en la posición 1 obliga a desplazar los elementos posteriores (`Array.Copy`), es decir, O(n).

### 5.6 Ventana principal: `MainForm`

Tiene una interfaz en español con tema oscuro, dividida en paneles:

- Registro: campos de título, artista, BPM y duración. Valida que título y artista no estén vacíos y muestra un aviso si faltan.
- Modo de colección: tres botones de opción que cambian en vivo entre la lista personalizada, `LinkedList<T>` y `List<T>`. Al cambiar, se reconstruye la colección conservando los datos actuales.
- Acciones de la cola: seis botones (Agregar al Final, Reproducir Siguiente, Avanzar Canción, Invertir Lista, Ordenar por Curva de BPM, Quitar Duplicados).
- Lista en vivo: `DataGridView` con la cola, una etiqueta de "Reproduciendo" (la cabeza), el total de canciones y el tiempo total acumulado.
- Telemetría de benchmark: botón que ejecuta 25,000 inserciones a mitad de lista sobre las tres colecciones, mide cada una con `Stopwatch` y muestra los tiempos junto con la justificación teórica.

Detalles de comportamiento:

- Los duplicados se determinan por título y artista, sin distinguir mayúsculas de minúsculas.
- Al avanzar con la cola vacía se muestra un mensaje informativo en lugar de una excepción no controlada.
- "Ordenar por BPM" extrae las canciones, vacía la cola y las reinserta con `InsertSorted` en orden ascendente.

### 5.7 G Player (`PlayerForm`)

Reproductor de audio independiente que se abre junto con el programa. Usa Windows Media Player por COM (enlace tardío), sin paquetes NuGet. Incluye:

- Reproducir/pausar, detener, anterior, siguiente y apertura de archivos.
- Barra de progreso con posibilidad de saltar a una posición y tiempo transcurrido/total.
- Control de volumen (0–100 %) y de tempo (50–150 %) con botón de restablecer.
- Lista de reproducción con doble clic para reproducir, botón de limpiar y soporte para arrastrar y soltar archivos.
- Avance automático a la siguiente canción al terminar la actual.
- Logo "G" dibujado por código, usado también como ícono de la ventana.
- Formatos admitidos: mp3, wav, wma, m4a, aac, flac y ogg (según los códecs instalados en Windows).

Si Windows Media Player no está disponible, los controles se deshabilitan y se muestra una advertencia.

### 5.8 Red multicomputadora (`Networking`)

Permite que varias computadoras compartan la misma cola:

- Topología en estrella: una PC actúa como anfitrión (servidor TCP) y las demás como clientes.
- Puerto predeterminado: 5050.
- Cada 400 ms se compara la cola local con la última versión conocida; si cambió, se envía la lista completa en formato JSON (una línea por mensaje).
- El anfitrión retransmite los cambios a los demás clientes, y un cliente nuevo recibe la lista actual al conectarse.
- Prevalece el último cambio recibido y se ajusta el siguiente `Id` para evitar colisiones entre equipos.
- Observa la cola de `MainForm` por reflexión, por lo que no modifica las clases existentes.
- La ventana de red muestra las IP locales, el estado de conexión y un registro de eventos.

Uso:en la PC principal elegir "Ser anfitrión" y conectar; en las demás elegir "Unirme a otra PC", escribir la IP del anfitrión y conectar. Ambos equipos deben estar en la misma red y el puerto debe permitirse en el Firewall de Windows del anfitrión.

## 6. Relación con la rúbrica

| Criterio | Peso | Ubicación |
|---|---|---|
| Punteros y Nodos | 40 % | `Node.cs` y los seis métodos de `SinglyLinkedList.cs` |
| Contraste con .NET 10 | 20 % | `IPlaybackQueue<T>` y los dos adaptadores; selector de modo en la interfaz |
| Telemetría real | 15 % | `BtnBenchmark_Click` y `MeasureMidListInsertion` en `MainForm.cs` |
| Bono de presentación | 25 % | `MainForm.Designer.cs`, más G Player y el módulo de red |

## 7. Casos de prueba

| Caso | Acción | Resultado esperado |
|---|---|---|
| CP-01 | Registrar una canción y usar "Agregar al Final" | Aparece al final de la tabla. |
| CP-02 | Registrar una canción y usar "Reproducir Siguiente" | Aparece en la fila 2, detrás de la cabeza. |
| CP-03 | "Avanzar Canción" | La cabeza se reproduce y sale de la tabla. |
| CP-04 | Cargar BPM 100, 110, 120 e "Invertir Lista" | La tabla muestra 120, 110, 100. |
| CP-05 | Cargar BPM 128, 115, 140, 120 y "Ordenar por Curva de BPM" | Queda 115, 120, 128, 140. |
| CP-06 | Cargar títulos repetidos y "Quitar Duplicados" | Se conserva el primero y se eliminan los demás. |
| CP-07 | "Avanzar Canción" con la cola vacía | Mensaje informativo, sin excepción no controlada. |

## 8. Resultado esperado del benchmark

| Colección | Complejidad | Motivo |
|---|---|---|
| Lista personalizada (nodos) | O(1) | Solo reconecta dos referencias por inserción. |
| `LinkedList<T>` de .NET | O(1) | Nodos doblemente enlazados nativos. |
| `List<T>` de .NET | O(n) | Arreglo contiguo que desplaza elementos con `Array.Copy`. |

Los tiempos exactos dependen del equipo en que se ejecute.

## 9. Alcances y limitaciones

Este proyecto se entregó con las siguientes limitaciones, que reconocemos abiertamente:

- G Player no está integrado con la cola de SoundCore: es un reproductor independiente con su propia lista de archivos y no reproduce las canciones registradas en el controlador.
- Los datos no se guardan: la cola vive solo en memoria y se pierde al cerrar la aplicación.
• Sincronización en red simple: cada cambio envía la lista completa y el último cambio recibido sobrescribe al anterior, sin resolución de conflictos ni cifrado de la comunicación. Depende de la reflexión sobre campos privados de `MainForm`, lo que la hace frágil ante cambios en esa clase.
- Benchmark en el hilo de la interfaz: la prueba de 25,000 inserciones se ejecuta en el hilo principal, por lo que la ventana puede quedar momentáneamente sin responder.
- Sin pruebas automatizadas: los casos CP-01 a CP-07 se verifican manualmente.
- Compatibilidad: solo funciona en Windows y el reproductor requiere Windows Media Player.
- Comentarios y documentación: parte del código conserva comentarios en inglés, mientras que la interfaz y los documentos complementarios están en español.

## 10. Autoevaluación

Como equipo nos asignamos una calificación de 85/100. Consideramos que el proyecto cumple con los algoritmos centrales, el contraste con las colecciones nativas y la telemetría solicitada, pero le faltaron aspectos por completar (varios de los señalados en la sección anterior) y fue entregado fuera del tiempo establecido. Esta nota refleja de manera honesta tanto el trabajo realizado como los pendientes y el incumplimiento en la fecha de entrega.

## 11. Créditos

Proyecto desarrollado por:
👤Alexa Abigail Fraire Sandoval
👤Celeste Anyelique Garza Mauricio
para la materia de Estructuras de Datos del TecNM Campus Monclova.
