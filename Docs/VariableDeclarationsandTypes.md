Next: [***Function Declarations >>***](a.com)<br>
Previous: [***Creating a new project >>***](a.com)
<br>
# Variable Declarations and Types
Declaring a variable inline uses the same JavaScript method, but there are other extension methods for variables that use multiple lines.
## var (Generic Inline Variables)
The most common type of variable. To be declared, just use the conventional syntax: **

> **var MyVariable = MyContent**

It can be any value, text enclosed in single quotes (**'**) or double quotes (**"**), a number, or ***Boolean*** notation.

## obj (Object Variables)
Objects are composite variables that always contain two values per section: a key and a content. The declaration of objects (just like the declaration of arrays) is defined in the first line, its contents inside and then ended with a specific indicator following the syntax:

>**obj MyObject**<br>
>**a: 'Content of a'**<br>
>**b: 'Content of b'**<br>
>**c: 'Content of c'**<br>
>**end-obj**

## array (Vector Variables)
Arrays (vectors) are composite variables that are declared in multiple lines just like objects, but their difference is that they only hold values and not declarative keys following the syntax:

>**array MyArray**<br>
>**'Content 1'**<br>
>**'Content 2'**<br>
>**'Content 3'**<br>
>**end-array**

## mstring (Multi-line strings)
Alternative to using generic variables with multi-line (`) initializers that follow the syntax:

>**mstring MyMultilineString**<br>
>**This is an example of<br>
>multiline string usage<br>
>yeah.**<br>
>**end-mstring**<br>

