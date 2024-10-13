# Solution 
Since the output structure was tree like (JSON Tree); I noticed that each '(' indicated new identation in tree while ',' indicated node at the same layer. I split the string in node structure and passed the desired structure order
as input for the highest level heirarchy. In Output 2 I noticed that for 'type' the sub-tree ordering was not similar to original provided so I introduced an enum indicating what sorting we require for child nodes of 'type' key.
