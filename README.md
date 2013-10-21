xna-broad-engine
================

A simple XNA engine to help speed up XNA game project development and practice C# programming at the same time.
Big thanks to Eric for inspiring most of the core ideas for this library and the 1st 49er Game Jam to help me realize how it could be improved upon.

Features:

- [ ] Event driven model
- [ ] Activities act as screens and can be loaded individually
- [ ] A game wrapper class is run with an initial activity
- [ ] An activity manager class handles the current activity and allows activities to be queued up before forcing a change
- [ ] A screen class handles all drawing and camera shenanigans
- [ ] Game objects are attached to activities to update and be drawn individually
- [ ] Game objects can be attached to other game objects to act as modules and influence them as such
- [ ] Input is handled by linking keys to an enum for a game object, and can be disabled on a per object basis
- [ ] Looped animations and animation transitions will be supported
- [ ] Built-in menus and formatted text will be supported
- [ ] Collision can be based on one or more of bounding boxes, bounding circles, or pixel perfect checks
- [ ] Light / visibility maps will be supported
- [ ] Classes loaded via xml files will be supported
- [ ] Content will be loaded via a single text file specifying the type, name in game, and file name
- [ ] Many extensions
