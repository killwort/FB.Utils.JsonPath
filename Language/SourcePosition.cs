namespace FB.Utils.JsonPath.Language {
    public struct SourcePosition {
        public SourcePosition(int line, int column) {
            Column = column;
            Line = line;
        }

        public int Column;
        public int Line;
    }
}