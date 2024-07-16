@Since: October 2017
@Author: Peter Metz
@License: MIT

# What does it do?

Populating values between related class instances for Properties and Fileds. Does also populate correclty values for immutable types like Enum, ...

Relation of classes can by same names and types for Properties or Fields.
Relation of classes can be also implementing same interface - this allows to populate only certain values.

# Helpers

CultureInfoHelper - speeds up the search for LCID to language or code page
Experience with using the .NET native implementation show a delay of up to 3 millisecond for that kind of mapping
(thats 0.003 * 2 000 000 000 = 6 000 000 ticks - that's CRAZY Microsoft stuff!)

PropertyValueHelper - library providing methods to copy values from one class intance to another class instance, when 
both do implement same interface, works also for child interfaces.
That method copies same Proptery names and Types between two class instances

StringTypeValueConverter - library providing method to deserialize serialized value as string and type name as string
