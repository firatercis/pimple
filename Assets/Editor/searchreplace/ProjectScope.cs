namespace sr
{
  [System.Flags]
  public enum ProjectScope
  {
    EntireProject,
    SpecificLocation,
    CurrentScene,
    CurrentSelection
  }
}