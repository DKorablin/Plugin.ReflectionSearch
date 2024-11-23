# Reflection Search plugin
This plugin serves as an extension for plugins that support certain data retrieval methods.
List of required methods and returns types. List required methods and required data types:

    Type GetEntityType() — Get the type of the object that is used to search for the object
    String[] GetSearchObjects(String folderPath) — Get an array of objects to search for
    Object CreateEntityInstance(Object filePath) — Create an instance of an object to search through reflection (Expected type must be the same as type from the method GetEntityType), passing as an argument one element from the array obtained from the method GetSearchObject.