import javax.swing.*;
import java.awt.*;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.awt.geom.GeneralPath;
import java.util.Random;

public class TaskFrame extends Frame {

    private Color lineColor = Color.red;
    private Random random = new Random();
    private GeneralPath path = new GeneralPath();
    private float thickness = 2;
    private int cap = 1;
    private float[] dashes = {random.nextFloat(), random.nextFloat()};
    private boolean dash = false;

    public TaskFrame() {
        Label l = new Label("Nazar Yuras AMI-32 Task #15");
        l.setBounds(10, 10, 200, 80);
        add(l);

        setSize(500, 500);
        setTitle("Sinusoidal Spiral");
        setLayout(null);
        setVisible(true);

        addWindowListener(new WindowAdapter() {
            public void windowClosing(WindowEvent e) {
                dispose();
            }
        });

        addComponentListener(new ComponentAdapter() {
            public void componentResized(ComponentEvent componentEvent) {
                repaint();
            }
        });

        addMouseListener(new MouseAdapter() {
            public void mouseClicked(MouseEvent e) {
                do {
                    thickness = random.nextFloat() * 10;
                }
                while (thickness == 0);

                lineColor = new Color(random.nextFloat(), random.nextFloat(), random.nextFloat());
                cap = random.nextInt(3);
                if (random.nextFloat() > 0.5) {
                    dashes[0] = random.nextFloat() * 10;
                    dashes[1] = random.nextFloat() * 10;
                    dash = true;
                } else {
                    dash = false;
                }
                repaint();
            }
        });
    }

    @Override
    public void paint(Graphics g) {
        Graphics2D g2d = (Graphics2D) g;

        g2d.drawLine(0, this.getSize().height / 2, this.getSize().width, this.getSize().height / 2);
        g2d.drawLine(this.getSize().width / 2, 0, this.getSize().width / 2, this.getSize().height);

        g2d.translate(this.getSize().width / 2, this.getSize().height / 2);

        g2d.setColor(this.lineColor);
        g2d.setStroke(new BasicStroke(WIDTH, cap, 0));

        double coef = 20;
        int points = 360;
        int a = 5;
        double m = 0.856666;
        double mReversed = 1.166;
        for (int i = 0; i < points; i++) {
            double fi1 = 14 * Math.PI / points * i;
            double fi2 = 14 * Math.PI / points * (i + 1);
            double r1 = a * Math.pow(Math.cos(m * fi1), mReversed);
            double r2 = a * Math.pow(Math.cos(m * fi2), mReversed);
            double x1 = r1 * Math.cos(fi1) * getSize().width / coef;
            double y1 = r1 * Math.sin(fi1) * getSize().height / coef;
            double x2 = r2 * Math.cos(fi2) * getSize().width / coef;
            double y2 = r2 * Math.sin(fi2) * getSize().height / coef;
            g2d.drawLine((int) x1, (int) y1, (int) x2, (int) y2);

            if (!dash)
                g2d.setStroke(new BasicStroke(thickness, cap, 0));
            else
                g2d.setStroke(new BasicStroke(thickness, cap, 0, 10.0f, dashes, 0));

            int w = getWidth() / 4;
            int h = getHeight() / 4;
            path.reset();
            path.moveTo(w, h);
        }
    }
}
