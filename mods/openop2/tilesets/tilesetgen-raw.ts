function download(filename: string, text: string) {
    var pom = document.createElement('a');
    pom.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
    pom.setAttribute('download', filename);

    if (document.createEvent) {
        var event = document.createEvent('MouseEvents');
        event.initEvent('click', true, true);
        pom.dispatchEvent(event);
    }
    else {
        pom.click();
    }
}

type Tileset = {
    size: number,
    image: string,
    category: string,
    palette: string | null,
}

const filename = 'raw.yaml';
const tilesetName = 'Raw';
const category = 'Raw';

const tilesets: Tileset[] = [
{
    size: 1,
    image: 'well0000.bmp',
    category: category,
    palette: 'well0000.bmp',
},
{
    size: 269,
    image: 'well0001.bmp',
    category: category,
    palette: null,
},
{
    size: 163,
    image: 'well0002.bmp',
    palette: 'well0002.bmp',
    category: category,
},
{
    size: 6,
    image: 'well0003.bmp',
    palette: 'well0003.bmp',
    category: category,
},
{
    size: 359,
    image: 'well0004.bmp',
    palette: 'well0004.bmp',
    category: category,
},
{
    size: 147,
    image: 'well0005.bmp',
    palette: 'well0005.bmp',
    category: category,
},
{
    size: 54,
    image: 'well0006.bmp',
    palette: 'well0006.bmp',
    category: category,
},
{
    size: 207,
    image: 'well0007.bmp',
    palette: 'well0007.bmp',
    category: category,
},
{
    size: 347,
    image: 'well0008.bmp',
    palette: 'well0008.bmp',
    category: category,
},
{
    size: 141,
    image: 'well0009.bmp',
    palette: 'well0009.bmp',
    category: category,
},
{
    size: 96,
    image: 'well0010.bmp',
    palette: 'well0010.bmp',
    category: category,
},
{
    size: 150,
    image: 'well0011.bmp',
    palette: 'well0011.bmp',
    category: category,
},
{
    size: 72,
    image: 'well0012.bmp',
    palette: 'well0012.bmp',
    category: category,
},
];

let tileNum = 0;
let str = `General:
\tName: ${tilesetName}
\tId: ${tilesetName.toLocaleLowerCase()}
\tEditorTemplateOrder: Terrain
\tSheetSize: 1024

Terrain:
\tTerrainType@Clear:
\t\tType: Clear
\t\tColor: B59582
\t\tTargetTypes: Ground
\tTerrainType@ResourcePlaceholder:
\t\tType: ResourcePlaceholder
\t\tColor: 192326
\t\tTargetTypes: Ground

Templates:`;
for (let tilesetIndex = 0; tilesetIndex < tilesets.length; tilesetIndex++) {

    const { size, image, palette, category } = tilesets[tilesetIndex];

    for (let i = 0; i < size; i++) {
        const id = tileNum + i;
        let template = `
\tTemplate@${id}:
\t\tId: ${id}
\t\tImages: ${image}
\t\tSize: 1,1
\t\tFrames: ${i}
\t\tCategories: ${category}
\t\tTiles:
\t\t\t0: Clear`;

        if (!!palette) {
            template += `
\t\tPalette: ${palette}`
        }

        str += template;
    }

    tileNum += size;
}

download(filename, str);