Necromaniacal is a submission to Ludum Dare's April 2024 Game Jam
Original music was composed by Rosko
The game was created in Unity and coded in Visual Studio 2022

Check out the game's code here: github.com/Nathan-Amiri/Necromaniacal/tree/main/Assets/Scripts
Play the game here: machine-box.itch.io/necromanical

Due to personal reasons, the team's artist was forced to leave the project partway through the game jam, so the project was downscaled significantly in order to meet the submission deadline.

The PhaseManager class acts as the primary game manager, triggering the start and end of the game. Unique 'Phases' were planned, but didn't make it into the final submission due to downscaling.
The Entity class is the base class for all classes that have movement: Player, Enemy, and Minion. There is a unique Class for each Enemy & Minion which handles specific behavior.
All Entities have an Aura object, which turns all collided Tiles light or dark, depending on whether the Entity is aligned with or against the Player.
