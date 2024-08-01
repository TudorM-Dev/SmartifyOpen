#include <Servo.h>

Servo servo1;
Servo servo2;
Servo servo3;
// Servo servo4;

void setup() {
    Serial.begin(9600);
    servo1.attach(9); // Attach servo 1 to pin 9
    servo2.attach(10); // Attach servo 2 to pin 10
    servo3.attach(11);  // Attach servo 3 to pin 11
    // servo4.attach(12); // Attach servo 4 to pin 12
}

void loop() {
    if (Serial.available() > 0) {
        String message = Serial.readStringUntil('\n');
        if (message.length() > 0) {
            // Parse servo positions from the message
            int pos1 = message.substring(0, message.indexOf(',')).toInt();
            message = message.substring(message.indexOf(',') + 1);
            int pos2 = message.substring(0, message.indexOf(',')).toInt();
            message = message.substring(message.indexOf(',') + 1);
            int pos3 = message.substring(0, message.indexOf(',')).toInt();

            // Move servos to the desired positions slowly
            moveServoSlowly(servo1, pos1);
            moveServoSlowly(servo2, pos2);
            moveServoSlowly(servo3, pos3);

            // Serial.println("Servo positions set to: " + String(pos1) + "," + String(pos2) + "," + String(pos3));
        }
    }
}

void moveServoSlowly(Servo &servo, int targetPos) {
    int currentPos = servo.read();
    if (currentPos < targetPos) {
        for (int pos = currentPos; pos <= targetPos; pos++) {
            servo.write(pos);
            delay(5); 
        }
    } else {
        for (int pos = currentPos; pos >= targetPos; pos--) {
            servo.write(pos);
            delay(5); 
        }
    }
}
