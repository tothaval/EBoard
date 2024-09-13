# EBoard
successor to AEUI and YSUI, will be created using the MVVM pattern and C#
<br>
<br>
EBoard is a successor to AEUI (2024, Aiding Elements User Interface) and YSUI (2022, YRS, Your Startup UI),
will be created using the MVVM pattern and C# with WPF and XAML.
<br>
<br>
Current Features:<br>
<br>
<br>
'!', 'Ciao' and 'Off' buttons on top close the application, there is no warning!
<br>
a right click anywhere except on an element opens the eboard context menu that currently
supports minimizing, maximizing, normalizing and quitting the application.
left click and hold anywhere except buttons and elements will drag move the entire window.
<br>
within the window below the top buttons is the eboard selection and below that the currently
selected eboard instance, which is basically a WPF canvas. 
<br>
atm there are 3 eboards to prove instantiation selectable from eboard selection, the eboards can be switched via leftclick
on the button next to name, the background color in the selection field is that of the eboard instance itself.
there are 3 elements per eboard to prove element instantiation, each element can be drag moved via leftclick and hold,
while dragging, the element gets a z-index of 1000, after that, z-index is reset to initial value, which is 0 atm.
right click on an element opens a context menu, where the element can be deleted.
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
