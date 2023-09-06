# Vanlife Grid System

## Introduction
This project is a vanilla version of the grid system designed for creating vanlife layouts. This is a basis of future algorithms to procedually generate the layout.  
The project allows users to select and place items on a grid interactively. Items are designed to rotate around their bottom-left origin. 

## Controls
- **Left Click on the left panel**: Select the item.
- **Hover on the grids**: Highlight the grid
- **Left Click on the grids**: Place the selected item onto the grid. The origin of the item (bottom left) will be put on the highlighted grid.
- **Click the rotation button OR R button on keyboard**: Rotate the selected item clockwise. The button's text shows where the origin is.
  
## Classes & Key Components
- **SC_GridSystem**: Main class handling the creation and interaction with the grid. It uses a flattened matrix to manage the grids.
- **SC_GridCell**: Represents individual grid cells.
- **SC_Item**: Represents individual items that can be placed on the grid.
- **SO_Item**: The "config file" for each item.
- **SC_ItemBrowser**: Handles the browsing and selection of items using scroll view and grid group.

## SO_Item Configuration Guide
### Creating a new SO_Item
1. **In Unity Editor:**  
   Right-click in the Project tab > Create > GridSystem > Item.

2. **Configure the SO_Item:**  
   - **Sprite**: Assign a sprite that visually represents the item.
   - **OccupiedSlots**: Define the grid slots that this item will occupy. This should be a list of 2D coordinates (Vector2Int) which represents the relative positions this item occupies on the grid. The origin is bottom left corner.

## Development Guide
- **SO Sprite:** The sprite for each "cell" is 500 * 500 pixels. Therefore, if your table's size is 2 * 1, please make the image size 1000 * 500. Make sure the sprite's **pixels per unit** is 500. If the sprite and the SO are set up correctly, it should be displayer properly automatically.
