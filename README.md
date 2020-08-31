# DynamicCursor
Cursor icon that follows your mouse direction

Preview:


![preview](https://s7.gifyu.com/images/cursor.gif)

## Installation
* [Download](https://github.com/Gaareth/DynamicCursor/releases) the exe (or compiled it yourself) 
* Clone this repository
* Copy the exe into the cloned repo
* => Start the exe


## WARNING
The dynamic cursor works by modifying the registry entry of the default cursor.
By using the exit button in the SystemTray the registry entries will be reverted to: <br>
                
```
{"Arrow", "C:\\Windows\\Cursors\\aero_arrow.cur"},
{"Hand",  "C:\\Windows\\Cursors\\aero_link.cur"}
```

If these are not your default cursors the cursors may be a bit different. By changing the Windows cursor size (System > Cursor size) everything should be back to normal.
***

### Generating
If you want to generate the cursor files yourself you need to:
* Install [python](https://www.python.org/downloads/)
* Install the required packages: `pip install -r requirements.txt` <br> Note: `opencv-python` and `numpy` are optional
* Clone [Iconolatry](https://github.com/SystemRage/Iconolatry) for the .cur conversion
#### Usage
`> python cursor_gen.py -i BASE_IMAGE -o OUTPUT_DIR` <br>
Note: the OUTPUT_DIR should be named `rotated` (If the base cursor is black: `rotated_black`)
***

Also a SystemTray is included, with options to:
* Change the color of the cursor (A second folder called `rotated_black` is required)
* Stop the animated cursor
* Exit the programm
