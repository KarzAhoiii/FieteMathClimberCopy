0. Package importieren

0.5 Wenn noch nicht im Projekt: Leantween importieren

1. Prefab in die Intro-Szene ziehen. �ber das Script auf dem "Loading Screen" Objekt k�nnen Geschwindigkeit der �berg�nge und Animationskurven festgelegt werden.

2. Farbe der Ladebildschirmkompenten kann �ber die einzelnen Sprite und Image GameObjects geregelt werden. Bild von Fiete auch.

3. Ein neues "Layer" anlegen. Alle Objekte des Prefabs auf diese Ebene setzen und die "Pinhole Camera" nur diese Ebene rendern lassen.

4. Alle anderen Kameras diese Layer nicht rendern lassen.

5. Falls nicht gesetzt: Das Material "SpritePinholeMaterial" auf f�r das "Pinhole" Objekt setzen.

6. Anwendung: Einfach �ber FLS_LoadingScreen.instance.loadLevel(string levelName) ein neues Level laden. Ist ein Singleton also von �berall aus erreichbar.