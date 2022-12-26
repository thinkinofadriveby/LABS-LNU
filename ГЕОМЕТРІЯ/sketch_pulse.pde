char angle = 0;

void setup() {
  size(1000, 1000);
  background(102);
  //noStroke();
  fill(0, 102);
}

void draw() {
  // Draw only when mouse is pressed
  if (mousePressed == true) {
    angle += 5; //швидкість пульсу
    float val = sin(radians(angle)) * 50.0; // коефіціент (зміщення кругів + їх розмір) при кожному малюванні
    for (int a = 0; a < 360; a += 72) { // відповідає за розміщення 5 кругів
      float xoff = cos(radians(a)) * val; //
      float yoff = sin(radians(a)) * val; //
      fill(255);
      ellipse(mouseX + xoff, mouseY + yoff, val, val);
    }
    fill(255);
    ellipse(mouseX, mouseY, 2, 2);
  }
}