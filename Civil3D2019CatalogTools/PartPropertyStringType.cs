namespace Civil3d.CatalogTools
{
    /// <remarks>
    /// Если название начинается с Pipe - параметр применим только
    /// к трубам, если со Structure - только к колодцам.
    /// Если в названии содержится Unknown - это значит что не
    /// удалось определить назначение параметра или примерно значение
    /// понятно, но дополнительные исследования для точного определения
    /// не проводились.
    /// Если на конце число (1, 2, 3...) - значит, что такой параметр
    /// неуникален
    /// </remarks>
    public enum PartPropertyStringType
    {
        PartDescription = 67112965,
        PartName = 67112979,
        /// <summary>
        /// При каждом последующем запросе это значение увеличивается на 1.
        /// При этом изменяется какой-то объект AeccDbNameTemplatesNode.
        /// Проверил нумераторы имён труб и колодцев - они не изменяются.
        /// Изменённое значение сохраняется в чертеже.
        /// Этот параметр является общим для всех элементов чертежа одного типа:
        /// отдельное значение для труб, отдельное - для колодцев
        /// Закомментировал для предотвращения случайных вызовов - т.к.
        /// непонятно что это, но понятно, что при его использовании
        /// в чертёж вносятся изменения
        /// </summary>
        //UnknownCounter = 67112990,
        LayerName = 67112992,
        StyleName = 67112994,
        ColorName = 67113061,
        LineTypeName = 67113062,
        /// <summary>
        /// AeccDbPipe или AeccDbStructure
        /// </summary>
        ClassName = 67113065,
        /// <summary>
        /// Обнаруженное значение: Catalog Item
        /// </summary>
        PipeUnknown = 67477761,
        PipeStartStructureName = 67477832,
        PipeEndStructureName = 67477833,
        PipeName1 = 67477856,
        PipeName2 = 67477862,
        PipeName3 = 67477863,
        PipeDescription = 67477878,
        /// <summary>
        /// Возможные значения: B, Из
        /// </summary>
        StructureFirstConnectedPipeFlowDirection = 67481887,
        /// <summary>
        /// Возможные значения: С,СВ,В,ЮВ,Ю,ЮЗ,З и СЗ
        /// </summary>
        StructureFirstConnectedPipeDirection = 67481889,
        StructureFirstConnectedPipeShape = 67481891,
        StructureFirstConnectedPipeMaterial = 67481895,
        StructureFirstConnectedPipeStartStructureName = 67481897,
        StructureFirstConnectedPipeEndStructureName = 67481920,
        StructureFirstConnectedPipeName = 67481921,
        StructureFirstConnectedPipeSizeName = 67481922,
        /// <summary>
        /// Обнаруженное значение: 0
        /// </summary>
        StructureUnknownInt = 67481926,
        StructureDescription = 67481929,
        ReferencedAlignmentName = 67485969,
        ReferencedSurfaceName = 67485991,
        NetworkName = 67490088,
        /// <summary>
        /// Это значение используется внутренними механизмами
        /// Civil 3D API для получения Part.PartSizeName   
        /// </summary>
        PartSizeName1 = 67490097,
        /// <summary>
        /// Pipe_Domain или Structure_Domain
        /// </summary>
        DomainName = 69730504,
        PartType = 69730505,
        PartSubType = 69730506,
        CatalogFamilyName = 69730507,
        PartSizeName2 = 69730508,
        CatalogFamilyDescription = 69730509,
        CatalogGuid = 69730510,
        FamilyGuid = 69730512,
        CatalogSizeTableRowGuid = 69730515,
        // <summary>
        /// Обнаруженное значение: 2.0
        /// </summary>
        UnknownDouble = 69730520,
        Material = 69730604,
        PipeShapeName = 69730704,
        StructureShapeName = 69730804,
        /// <summary>
        /// Обнаруженное значение: Стандарт
        /// Скорее всего, это то, что в свойствах колодца
        /// отображается как "Коробка"
        /// </summary>
        StructureSizeUnknownParam1 = 69730818,
        /// <summary>
        /// Обнаруженное значение: Стандарт
        /// Скорее всего, это то, что в свойствах колодца
        /// отображается как "Решётка"
        /// </summary>
        StructureSizeUnknownParam2 = 69730819,
        /// <summary>
        /// Обнаруженное значение: Стандарт
        /// Скорее всего, это то, что в свойствах колодца
        /// отображается как "Покрытие"
        /// </summary>
        StructureSizeUnknownParam3 = 69730820,
        /*
         * 69731305-3269731323(возможно и далее - если добавить в
         * каталог ещё параметров) - уникальные параметры трубы, в том
         * числе и нестандартные
         */
    }
}
