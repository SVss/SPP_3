namespace XmlParserWpf.ViewModel
{
    // ITimed

    internal interface ITimed
    {
        uint Time { get; set; }
    }

    // IExpandable

    internal interface IExpandable
    {
        bool Expanded { get; set; }

        void ExpandAll();

        void CollapseAll();
    }

    // IChangeable

    public delegate void ChangeDelegate();

    internal interface IChangeable
    {
        event ChangeDelegate ChangeEvent;

        void OnChange();
    }
}
