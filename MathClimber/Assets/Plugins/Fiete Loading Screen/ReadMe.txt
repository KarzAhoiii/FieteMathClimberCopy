0. Package importieren

0.5 Wenn noch nicht im Projekt: Leantween importieren

1. Prefab in die Intro-Szene ziehen. Über das Script auf dem "Loading Screen" Objekt können Geschwindigkeit der Übergänge und Animationskurven festgelegt werden.

2. Farbe der Ladebildschirmkompenten kann über die einzelnen Sprite und Image GameObjects geregelt werden. Bild von Fiete auch.

3. Ein neues "Layer" anlegen. Alle Objekte des Prefabs auf diese Ebene setzen und die "Pinhole Camera" nur diese Ebene rendern lassen.

4. Alle anderen Kameras diese Layer nicht rendern lassen.

5. Falls nicht gesetzt: Das Material "SpritePinholeMaterial" auf für das "Pinhole" Objekt setzen.

6. Anwendung: Einfach über FLS_LoadingScreen.instance.loadLevel(string levelName) ein neues Level laden. Ist ein Singleton also von überall aus erreichbar.