# DynamicCursor
Cursor icon that follows your mouse direction

Preview:


![preview](https://s7.gifyu.com/images/cursor.gif)

## Installation
* [Download](https://github.com/Gaareth/DynamicCursor/releases) the exe
* Clone this repository
* Copy the exe into the cloned repo

### Generating
If you want to generate the cursor files yourself you need to:
* Install [python](https://www.python.org/downloads/)
* Install the required packages: `pip install -r requirements.txt` <br> Note: `opencv-python` and `numpy` are optional
* Clone [Iconolatry](https://github.com/SystemRage/Iconolatry) for the .cur conversion
#### Usage
`> python cursor_gen.py -i BASE_IMAGE -o OUTPUT_DIR`

Also a SystemTray is included, with options to:
* Change the color of the cursor
* Stop the animated cursor
* Exit the programm
