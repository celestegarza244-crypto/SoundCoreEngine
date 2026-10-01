# Red multi-computadora (SoundCore)

Se agregó la carpeta `Networking/` (NetworkSync.cs y NetworkForm.cs). No se
modificó ninguna clase existente; solo `Program.cs` abre la nueva ventana "Red".

## Uso
1. En la PC principal: ventana "Red" → "Ser anfitrión" → Conectar.
2. En las demás PCs (misma red Wi-Fi/LAN): "Unirme a otra PC", escribir la IP
   del anfitrión (aparece en su ventana como "Mi(s) IP") → Conectar.
3. Cualquier cambio (agregar, avanzar, invertir, ordenar, duplicados) se
   replica en todas las computadoras en ~0.4 s.

Puerto por defecto: 5050 (TCP). Permitirlo en el Firewall de Windows del anfitrión.
