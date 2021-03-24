
let str = '';

for (let i = 32; i < 269; i++) {

    const id = i.toString();
    const template = `
\tTemplate@${i +1}:
\t\tId: ${i + 1}
\t\tImages: well0001.png
\t\tSize: 1,1
\t\tFrames: ${i}
\t\tCategories: Terrain
\t\tTiles:
\t\t\t0: Clear`;
    str += template;
}

console.log(str);