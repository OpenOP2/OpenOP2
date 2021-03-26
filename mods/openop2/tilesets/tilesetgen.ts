let str = '';

type Tileset = {
    size: number,
    image: string,
    category: string,
    palette: string | null,
}

const tilesets: Tileset[] = [
{
    size: 269,
    image: 'well0001.bmp',
    category: 'Moon',
    palette: null,
},
{
    size: 156,
    image: 'well0002.bmp',
    palette: 'well0002.bmp',
    category: 'Moon',
},
{
    size: 6,
    image: 'well0003.bmp',
    palette: 'well0003.bmp',
    category: 'Moon',
},
{
    size: 359,
    image: 'well0004.bmp',
    palette: 'well0004.bmp',
    category: 'Rocky',
},
{
    size: 147,
    image: 'well0005.bmp',
    palette: 'well0005.bmp',
    category: 'Rocky',
},
{
    size: 54,
    image: 'well0006.bmp',
    palette: 'well0006.bmp',
    category: 'Transitions',
},
{
    size: 207,
    image: 'well0007.bmp',
    palette: 'well0007.bmp',
    category: 'Lava',
},
{
    size: 347,
    image: 'well0008.bmp',
    palette: 'well0008.bmp',
    category: 'Mars',
},
{
    size: 141,
    image: 'well0009.bmp',
    palette: 'well0009.bmp',
    category: 'Mars',
},
{
    size: 96,
    image: 'well0010.bmp',
    palette: 'well0010.bmp',
    category: 'Transitions',
},
{
    size: 150,
    image: 'well0011.bmp',
    palette: 'well0011.bmp',
    category: 'Icy',
},
{
    size: 72,
    image: 'well0012.bmp',
    palette: 'well0012.bmp',
    category: 'Icy',
},
];

let tileNum = 0;
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

console.log(str);