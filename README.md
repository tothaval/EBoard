
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
