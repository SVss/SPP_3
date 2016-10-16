namespace XmlParserWpf.ViewModel
{
    // ITimed

    interface ITimed
    {
        long Time { get; set; }
    }

    // IExpandable

    interface IExpandable
    {
        bool Expanded { get; set; }

        void ExpandAll();

        void CollapseAll();
    }

    // IChangeable

    public delegate void ChangeDelegate();

    interface IChangeable
    {
        event ChangeDelegate ChangeEvent;

        void OnChange();
    }
}
