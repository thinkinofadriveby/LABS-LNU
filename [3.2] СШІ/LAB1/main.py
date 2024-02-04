import os
import numpy as np
import matplotlib.pyplot as plt
from tensorflow import keras
from tensorflow.keras.layers import Dense, Flatten
from PIL import Image
from numpy import asarray, array


def process_initial_data(folder_path):
    folder = os.listdir(folder_path)
    marks = []
    images = []

    for folder_name in folder:
        for image in os.listdir(f"{folder_path}/{folder_name}"):
            marks.append(folder_name)
            images.append(asarray(Image.open(f"{folder_path}/{folder_name}/{image}").convert('L')))

    return array(images), array(marks)


(x_to_train, y_to_train) = process_initial_data('Train')
(x_to_recognize, y_to_recognize) = process_initial_data('Recogn')
x_to_train = x_to_train / 255
x_to_recognize = x_to_recognize / 255

classes = {"c": 0, "yS": 1, "X": 2, "yB": 3}

y_to_train = np.array([classes[i] for i in y_to_train])
y_to_recognize = np.array([classes[i] for i in y_to_recognize])
y_to_train_categorical = keras.utils.to_categorical(y_to_train, 4)

model = keras.Sequential([
    Flatten(input_shape=(16, 16, 1)), # масив зображень, 1 вхідний шар
    Dense(24, activation='sigmoid'), # 2 прихований шар
    Dense(12, activation='tanh'), # 3 прихований шар, функція активації
    Dense(4, activation='sigmoid') # вихідний шар
])

# print(model.summary())

model.compile(optimizer=keras.optimizers.SGD(learning_rate=0.01, momentum=0.9), # компіляція моделі
              loss='categorical_crossentropy', # функція втрат
              metrics=['accuracy']) # метрика точності

# навчання моделі
characteristics = model.fit(x_to_train, y_to_train_categorical, batch_size=4, epochs=24, validation_split=0.1)
plt.plot(characteristics.epoch, characteristics.history["loss"], 'red', label='LOSS')
plt.plot(characteristics.epoch, characteristics.history["accuracy"], 'goldenrod', label='ACCURACY')
plt.legend()
plt.title('Model characteristics:')
plt.xlabel('x')
plt.ylabel('y')
plt.grid()

try_to_recognize = model.predict(x_to_recognize)
try_to_recognize = np.argmax(try_to_recognize, axis=1)

# матриця похибок
print('\nConfusion matrix:')
print(f'\tRecognition result: {try_to_recognize[:12]}')
print(f'\tTrue result:        {y_to_recognize[:12]}')

print('\nImage recognition success:')
mask = try_to_recognize == y_to_recognize
print(mask[:12])

unrecognized_images_count = sum(not elem for elem in mask)
print(f'\n[!] Unrecognized images: {unrecognized_images_count}')

not_x = x_to_recognize[~mask]
not_y = y_to_recognize[~mask]

plt.figure(figsize=(10, 5))
plt.suptitle('Unrecognized images:', fontsize=14, fontweight='bold')
for i in range(len(not_x)):
    plt.subplot(5, 5, i + 1)
    plt.xticks([])
    plt.yticks([])
    plt.imshow(not_x[i], cmap=plt.cm.binary)

plt.show()
