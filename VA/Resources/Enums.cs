namespace VA.Resources
{
    public enum GridPathItemsState
    {
        Neutral, Start, Destination, Path, Search
    }

    public enum ModuleType
    {
        SortModule, StringMatchingModule, GridPathModule
    }

    public enum SortItemsState
    {
        Inactive, Active, Pivot
    }

    public enum StringMatchingItemsState
    {
        Inactive, Active, Space
    }
}