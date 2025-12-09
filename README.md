2025|12|09 this readme is outdated, anything from here until the end of this addition, which is marked by the same date, is new information, beyond information will be updated in the future. <br><br>

changes in current version may break previous configurations<br><br>

it is now possible to include new plugins via the basic build in plugin architecture by copying a proper
plugin dll file into the InstalledPlugins folder on executable folder.<br><br>

eboard in its current state will log erros to log files. keep an eye on the folder sizes now and then. but it is okay robust right now, and the logging mechanism isn't complete, so there is no imminent danger of too much space taken by this.<br><br>

every ui surface context has its own right-click context menu with different options, most if not all visible buttons should have the described effects and should work, there are also sliders, some need the hit of an OK button to have effects.
contexts are MainWindow, EboardBrowser, EboardScreen and EboardElement for the color and design change functions, additional contexts are subelemental controls like Textboxes, the element Views or certain parts of them. some context menus have titles showing too which context they belong<br><br>

some new controls, like the areaview, have red and blue buttons to their top left corner, blue will add a row or a column of a certain element type in the direction it faces, red will remove a row or a column in that direction. besides the mynote plugin, all area elements save and load changes to the areas or containing elements. there will be no warning upon deletion.<br><br>

it is possible to resize elements with CTRL+Left mouse pressed and drag, although this is still a mockup for a real resizing, the element context has to be hit, else it won't work. resizing can also be done via slider in Size->Area from within the context menu. Reset Size will change width and height back to NaN so that WPF autosizing can work again.<br><br>

it is possible to Rename StandardText titles with CTRL+Left Click onto the title. Enter will change it back to Textblock or readonly Textbox. <br><br>

it is possible to Rename Link element target names by right click onto the icon or Text. pressing Enter or clicking Edit on the Right click menu again will end renaming functionality.<br><br>

it is possible to retarget Link elements and image elements with CTRL+Left click.<br><br>

Left Click pressed and drag will move Elements when Element context is hit. In some cases, like Scrollviewers or Listboxes there is no hit detection. this is a low prio fix for later. it is the same reason why left click dragging does not work on eboard screens. this is attempted, because unless drawing is done on the screen, which is not yet implemented, there is no need that it can't be dragged. left click drag on MainWindow context will move the mainwindow. there is no window caption. to close the mainwindow, use windows, the off button, the shutdown button or close on MainWindow context menu.<br><br>

Shift+ LeftClick on an Element will select that element. there are some basic functions for group transformations like moving, rotating and arranging them into a line. Only degrees 0, 45, 90 on plus and minus scale work atm. in all other cases selected elements will be placed on top of each other.
to trigger selected group effects, use right click context menu on one of the selected objects or use Eboard Settings (which will be renamed into something better)<br><br>

i did not include any solution files for VS, you will have to setup a new solution and copy the projects there. i left the csproj files for guidance towards rebuilding, referencing and building the solution. <br><br>

eboard works and is fun so far. i am, however, no designer, so my initial color choices may be irritating.
just change its appearance by altering either the config files below the bin folder or by using the contextmenus on
the irritating context and recolor it to your liking. optical and design stuff is saved in .edf files, element content in .ecf files. the entire eboard and all of its contextes can be changed fast. you could probably test several completely different designs in under ten minutes once u know how to handle these changes and found the fastest workflow for you.<br><br>

there are sliders for black-grey-white, for red, green and blue, one for alpha value. a small rectangle will show the new color, pushing the ok button will apply it. be aware, there is no algorithm or feature in place yet to prevent same values for background and foreground colors. if somewhere values can not be seen, it is probably because of the foreground color being to close to its background.<br><br>

a few controls can not yet be changed, because i lack the necessary xaml deep knowledge to work around the underlying issues, which is mostly resource location and referenciation, as well as template overwriting.<br><br>

the Summoner element can summon any element upon entering the plugin name as in the element selection menu after the > sign, summoned elements and changes to them will not be saved atm.<br><br>

shape elements are unfinished, they have a border around them that is not really needed, (unless they are selected maybe).<br><br>

some elements are mock ups and placeholders, i use them to test certain things or need to finish other stuff first before working further on them. FSNavigator for instance can display a directory tree but has not yet implemented anything besides that. the Gold element lacks an gold image. you will need to look up one yourself. i couldn't remember the license origin, it was free, but i don't know more details than that. should be doable in under ten minutes. it is a nonsense element. the same could be achieved upon any changeable surface context by adding a background or border image.<br><br>

any element that lets you select stuff from a file or folder dialog will only link the selected file or folder. you do not need to worry that eboard deletes files or stuff if you delete an element. internally, the absolute path to the linked file is stored as string in a model, that is used to write that string to an xml file. eboard loads from these files on startup and will save its last state upon close or crash, although the latter is probably only partially true atm. has been a while since i looked into that.<br><br>

end of update 2025|12|09.
<br><br>
previous readme text:
<br><br><br><br>
<br><br><br><br>
the current state of the project is work in progress<br>
many areas of the code are not yet refactored, simple or robust, there are duplications and other bad elements
many features are training exercises or experiments and are hacked as mockups until better implementations are at hand
model, view and viewmodel separation is suboptimal, as well as code structure<br><br>

next steps will include refactoring and further development of modularization and plugin architecture, removal of code duplications and the like, improvisation of implementations, debugging, logging and better at best complete exception handling, so that eboard rather logs than crashes.<br><br>

updating this readme will also be done in an upcoming update<br><br>
EBoard is a successor to AEUI (2024, Aiding Elements User Interface) and YSUI (2022, YRS, Your Startup UI) and will be created using the MVVM pattern and C#/WPF<br><br>

Current Features:<br><br>

top Buttons<br>
'Off' button on top closes the application, there is no warning!<br><br>

'Eboard Browser' shows or hides eboardbrowser view, which is an overview over all existing eboard instances and it allows to create, edit or delete eboard instances<br><br>

the currently selected eboard instance is basically a WPF canvas. any number of eboards with any size, depth and name can be instantiated, to instantiate a new eboard, use 'AddEBoard' button in eboard browser, eboards can be switched clicking on the small eboard representation within the eboard browser. the background in the selection field is that of the eboard instance.

# EBoard
<br>
the current state of the project is work in progress<br>
many areas of the code are not yet refactored, simple or robust, there are duplications and other bad elements<br>
many features are training exercises or experiments and are hacked as mockups until better implementations are at hand<br>
model, view and viewmodel separation is suboptimal, as well as code structure<br>
<br>
next steps will include refactoring and further development of modularization and plugin architecture, removal of code duplications and the like,
improvisation of implementations, debugging, logging and better at best complete exception handling, so that eboard rather logs than crashes.
<br><br>
updating this readme will also be done in an upcoming update
<br>
EBoard is a successor to AEUI (2024, Aiding Elements User Interface) and YSUI (2022, YRS, Your Startup UI) and
will be created using the MVVM pattern and C#/WPF
<br>
<br>
Current Features:<br>
<br>
top Buttons
<br>
'Off' button on top closes the application, there is no warning!
<br>
<br>
'Eboard Browser' shows or hides eboardbrowser view, which is an overview over all existing eboard instances and
it allows to create, edit or delete eboard instances
<br>
<br>
the currently selected eboard instance is basically a WPF canvas. any number of eboards with any size, depth and
name can be instantiated, to instantiate a new eboard, use 'AddEBoard' button in eboard browser, eboards can be
switched clicking on the small eboard representation within the eboard browser. 
the background in the selection field is that of the eboard instance.
<br>

<br>
'+Elements' opens a menu to instantiate container elements
<br>
<br>
'+Shapes' opens a menu to instantiate shape elements
<br>
<br>
'+Tools' opens a menu to instantiate container elements, that do not save and load any content
<br>
<br>
a right click anywhere opens a context menu
<br><br>
mainwindow context menu supports:<br>
+image,image reset(to white background), minimizing, maximizing, normalizing and quitting the application
<br><br>
eboard context menu supports:<br> +image 
+image, image reset(to white background), switch to (first, previous, next or last) eboard, delete eboard,
<br><br>
eboardbrowser context menu supports:<br>
+image, image reset(to white background)
<br><br>
element context menu supports:<br>
change width, height, z and rotation of element and every selected element, rotation will unify every
rotation value, +image, image reset(to white background), delete element and every selected element
<br>
<br>
<br>
<br>
leftclick <br>
each element within an eboard can be drag moved via leftclick and hold, while dragging, the element gets a z-index of 1000,
after that, z-index is reset to initial value, which is 0 atm.
<br>
<br>
shift+leftclick <br>
select or deselect elements
<br><br>
<br>
<br>
MouseWheel on Element
change z-index of element (!! currently there is a conflict with scroll events if eboard window height is smaller than
eboard instance height)
<br>
<br>
Ctrl+MouseWheel
change rotation angle of element (!! currently there is a conflict with scroll events if eboard window height is smaller than
eboard instance height)
<br>
<br>
<br>
<b>ad-hoc license terms</b><br>
<p>
until a license has been chosen, you may 
use the software or parts of it under the following conditions:<br><br>
1.)
If you want to distribute or use the source code or a derived binary
of the EBoard project for commercial purposes, you need to contact
the project team for authorization and payment details.
You may use the source or a derived binary for non commercial 
purposes free of charge. In order to do so, copy this adhoc terms
and a link to the repository to any source code file that uses code
derived from this project and to the folder that holds the compiled source code.

2.)
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
</p>
