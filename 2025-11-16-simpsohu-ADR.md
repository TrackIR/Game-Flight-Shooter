# ADR-Hunter-Simpson: Use the Unity UI Toolkit for UI Design

**Status:** 
Accepted

**Context:**
We need to use the latest features of the Unity engine to keep up-to-date with industry standards and documentation. The Unity provided UI Toolkit is a streamlined UI builder that Unity has been phasing in for several years, while phasing out the old UI design method.

**Decision:**
We will use the Unity UI Toolkit as our standard method of creating UI.

**Options Considered:**
- Unity UI Toolkit
- Place UI elements into Heirarchy directly (Old UI method)
- Keep no UI standards (Do nothing)

**Rationale:**
Unity's UI Toolkit provides up-to-date features and documentation, and will maintain support by the Unity Developers for the foreseeable future. Placing UI elements into the Heirarchy directly is inefficient, clustered, and hard to maintain, and it's important that we follow a standard method so doing nothing is not an option.

**Consequences:**
- Team members need to learn the Unity UI Toolkit.
- Any UI created with the old method will need to be updated.
- All UI in the future will need to be made with the UI Toolkit.

**References:**
- [UI Toolkit documentation](https://docs.unity3d.com/6000.2/Documentation/Manual/UIElements.html)