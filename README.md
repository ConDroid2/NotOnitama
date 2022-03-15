# NotOnitama
A quick prototype for a digital version of Onitama.

# Goals and Solutions
My main goal with this project was to make a system that would allow new cards to be created without having to have entirely new art assets made.

I accomplished this by having card data stored as scriptable objects. These card scriptable objects basically just store a name and a list of Vector2's. The name is the name that will be displayed on the card, and the list of Vectors stores where the move card will allow you to move (so a vector of 1,1 will allow the piece to move up one and over 1).

In order to use that data, I created a prefab for a card object. The script controlling the object can be fed a MoveCard, and it will display the move's name, and it will fill in it's grid to show where that move will allow a piece to move.

With this system, I can make any number of new cards (that follow the base rules of the game) by simlpy clicking "Create -> MoveCard" and fill in the appropriate data, and the card will just work with no extra coding or asset creation.

# Enhancements
Since this is just a quick prototype, there are lots of things I would like to add. 

- I would like to add some kind of sandbox scene that would allow you to choose what cards are in your game so you can more easily test new cards created.
- I would like if the script that holds all the MoveCard scriptable objects was able to grab all the MoveCards automatically without having to manually give it the cards. (I have done a similar thing in another project, but it seemed a little bit out of scope for this prototype)
- I would like to create some kind of visual editor for the cards to make it even easier for designers to create new cards. With this, it might be smart to move to cards being stored as a JSON file as well.


