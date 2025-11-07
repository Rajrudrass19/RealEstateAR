Scripts Overview

1. Placements.cs
Handles AR surface detection and building model placement using ARRaycastManager.
Detects real-world planes, shows a placement reticle, and places the 3D building on tap.
Includes functions to reset placement and retrieve the placed object.

2. ModelController.cs
Controls rotation, scaling, and touch gestures for the placed building model.
Supports single-finger rotation, two-finger pinch scaling, and UI-based rotate/scale actions.

3. UIManager.cs
Handles all UI interactions and mode switching between AR placement and Explore mode.
Manages building placement, scaling, rotation, and spawns a first-person player to explore inside the model.

4. SimplePersonController.cs
Provides first-person navigation inside the virtual building.
Supports movement using keyboard or joystick and smooth camera rotation for immersive exploration.

Flow:
Scan → Detect plane → Place model → Rotate/Scale → Enter Explore → Walk inside → Exit Explore.

Tools: Unity 2021+, AR Foundation, C#, Android/iOS (ARCore/ARKit)

Team: Hema P, Rajeswari, Varshitha
(NIT Trichy – CSPE51 AR/VR Project)
