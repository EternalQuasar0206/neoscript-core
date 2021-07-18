# Variable Declarations and Types
Declaring a variable inline uses the same JavaScript method, but there are other extension methods for variables that use multiple lines.
## var (Generic Inline Variables)
The most common type of variable. To be declared, just use the conventional syntax: **

> **var MyVariable = MyContent**

It can be any value, text enclosed in single quotes (**'**) or double quotes (**"**), a number, or ***Boolean*** notation.

## obj (Object Variables)
Objects are composite variables that always contain two values per section: a key and a content. The declaration of objects (just like the declaration of arrays) is defined in the first line, its contents inside and then ended with a specific indicator following the syntax:

>**obj MyObject**
>**a: 'Content of a'**
>**b: 'Content of b'**
>**c: 'Content of c'**
>**end-obj**

## array (Vector Variables)
Arrays (vectors) are composite variables that are declared in multiple lines just like objects, but their difference is that they only hold values and not declarative keys following the syntax:

>**array MyArray**
>**'Content 1'**
>**'Content 2'**
>**'Content 3'**
>**end-array**
