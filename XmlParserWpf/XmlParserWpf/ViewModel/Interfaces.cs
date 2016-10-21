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

        void ExpandAll(object sender);

        void CollapseAll(object sender);
    }

    // IChangeable

    public delegate void ChangeDelegate();

    internal interface IChangeable
    {
        event ChangeDelegate ChangeEvent;

        void OnChange();
    }
}
