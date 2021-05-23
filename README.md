# Prusa-G-Code-File-Data-Viewer
I wanted to figure out how much a folder of G-Code files costs to print, so I wrote a program to calculate it for me.

# How it works
Prusa slicers export `.gcode` files (the file format 3D printers read and interpret) with some extra metadata at the end that tells various things about that print. 
Some of these things are, for example, the price of the print, how much material was used, and so on.

The problem I faced was that Prusa's `.gcode` file viewer didn't show any of this data, and I had a lot of files to go through and add up these numbers.

What this program does, is moves through each `.gcode` file in a folder, identifies this metadata, and then stores it in a list that the user can sort, remove
individual items, or export to a `.csv` file.

# How to use it
First, you need to pick what directory you want to load `.gcode` files from. If you pick one that doesn't have any files, the program will notify you.

After that, you must wait a few seconds for the data to load. While this process is multithreaded, it loads slowly because the program scans each line of the `.gcode`
file from top to bottom in search of the desired metadata, and that data is in the last few lines of the code. A more efficient way to tackle this problem would
be to read the file from the bottom up, but C# does not make this easy. That would be a worthy enhancement, however. (See [issue #1](https://github.com/pdnelson/Prusa-G-Code-File-Data-Viewer/issues/1))


Now that the files are loaded, here are some of the features this program has!
- Sort the items by name, spool cost, filament used, or filament used cost
- Typing "Delete" while a full row is selected will remove that item from the list
  - This automatically deducts from the total filament used and cost
- Loading another directory will add to the current list; if you want to start over, you must first click "Clear" to get rid of the old list, or use your "Delete" key
a bunch
- Double-clicking a cell will pop up more information about that print. Well, that's what it would do if I implemented more things... Right now, all it does is
show the information you can already see on the table.
- "Export" will export the list to a `.csv`
  - It is important to note that the files will be listed in the .csv in the same order they were in the folder, regardless of how they are currently
  sorted on the user interface
  - Totals will be tallied at the bottom

# Screenshots
![image](https://user-images.githubusercontent.com/48131480/119244896-e8a07e80-bb42-11eb-868d-8296f2e9e274.png)
