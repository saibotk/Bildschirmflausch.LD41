import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.Comparator;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;
import javax.swing.JFrame;
import javax.swing.JPanel;
import org.joml.Vector2i;
import javafx.util.Pair;

public class LD41Map {

	public static class Vertex {

		public final Room	r;
		public float		value;
		public Vertex		parent;

		public Vertex(Room r) {
			this.r = r;
			value = Float.POSITIVE_INFINITY;
			parent = null;
		}
	}

	public static class Edge {

		public final Vertex	r1, r2;
		public double		dist;

		public Edge(Vertex r1, Vertex r2) {
			this.r1 = r1;
			this.r2 = r2;
			dist = r1.r.distance(r2.r);
		}
	}

	public static class Room {

		Rectangle				bounds	= new Rectangle();
		// int width = 0, height = 0;
		// Vector2i pos = new Vector2i();
		Map<Vector2i, TileType>	tiles;

		public float distance(Room r) {
			// Vector2i center1 = new Vector2i(bounds.x + bounds.width / 2, bounds.y + bounds.height / 2);
			// Vector2i center2 = new Vector2i(r.bounds.x + r.bounds.width / 2, r.bounds.y + r.bounds.height / 2);
			// return (float) center1.distance(center2);
			return (float) Point.distance(bounds.x, bounds.y, r.bounds.x, r.bounds.y);
		}
	}

	public static enum TileType {
		GROUND, WALL, DOOR;
	}

	public static void main(String[] args) {
		Set<Room> rooms = new HashSet<>();
		rooms.addAll(generate().getKey());

		{
			JFrame frame = new JFrame();
			frame.setDefaultCloseOperation(3);
			JPanel panel = new JPanel() {

				private static final long serialVersionUID = 5954622107064881267L;

				@Override
				public void paintComponent(Graphics h) {
					super.paintComponent(h);
					Pair<Set<Room>, Pair<Vertex, Set<Edge>>> graph = generate();
					Graphics2D g = (Graphics2D) h;
					g.translate(getWidth() / 2, getHeight() / 2);
					g.scale(4, 4);
					for (Room room : graph.getKey()) {
						g.setColor(new Color(1f, 0f, 0f, 0.8f));
						g.fill(room.bounds);
					}
					g.setColor(Color.BLUE);
					g.setStroke(new BasicStroke(2));
					for (Edge e : graph.getValue().getValue()) {
						g.drawLine(e.r1.r.bounds.x, e.r1.r.bounds.y, e.r2.r.bounds.x, e.r2.r.bounds.y);
					}
					Room root = graph.getValue().getKey().r;
					g.setColor(Color.GREEN);
					g.fillRect(root.bounds.x, root.bounds.y, 2, 2);
				}
			};
			frame.add(panel);
			frame.setPreferredSize(new Dimension(800, 500));
			frame.pack();
			frame.setVisible(true);
			frame.repaint();
			frame.setExtendedState(6);

			frame.addKeyListener(new KeyListener() {

				@Override
				public void keyTyped(KeyEvent e) {
					rooms.clear();
					Pair<Set<Room>, Pair<Vertex, Set<Edge>>> graph = generate();
					rooms.addAll(graph.getKey());
					frame.repaint();
				}

				@Override
				public void keyReleased(KeyEvent e) {
				}

				@Override
				public void keyPressed(KeyEvent e) {
				}
			});
		}
	}

	public static Pair<Set<Room>, Pair<Vertex, Set<Edge>>> generate() {
		int minRoomSize = 50;
		Set<Room> rooms = new HashSet<>();
		for (int i = 0; i < 7 + (int) (Math.random() * 4); i++) {
			Room room = new Room();
			room.bounds.width = 15 + (int) (Math.random() * 20);
			room.bounds.height = 15 + (int) (Math.random() * 20);
			rooms.add(room);
		}

		outest: while (true) {
			for (Room r1 : rooms) {
				for (Room r2 : rooms) {
					if (r1 == r2)
						continue;
					Vector2i p1 = new Vector2i(r1.bounds.x + r1.bounds.width / 2, r1.bounds.y + r1.bounds.height / 2);
					Vector2i p2 = new Vector2i(r2.bounds.x + r2.bounds.width / 2, r2.bounds.y + r2.bounds.height / 2);
					if (p1.distanceSquared(p2) < 2 * minRoomSize * minRoomSize + 2) {
						r2.bounds.x += (int) ((Math.random() - 0.5) * 5);
						r2.bounds.y += (int) ((Math.random() - 0.5) * 2.5);
						continue outest;
					}
				}
			}
			break;
		}

		Set<Vertex> Q = new HashSet<>();
		for (Room r : rooms)
			Q.add(new Vertex(r));
		Vertex root = Q.stream().min(Comparator.comparing(v -> v.r.bounds.x)).get();
		root.value = 0;

		Set<Edge> E = new HashSet<>();
		Set<Edge> G = new HashSet<>();
		Set<Vertex> F = new HashSet<>();
		for (Vertex r1 : Q) {
			outer: for (Vertex r2 : Q) {
				if (r1 == r2)
					continue outer;
				for (Edge e : E)
					if (e.r2 == r1 && e.r1 == r2)
						continue outer;
				E.add(new Edge(r1, r2));
			}
		}
		F.add(root);
		Q.remove(root);

		while (!Q.isEmpty()) {
			Edge start = E.stream()
					.filter(e -> (F.contains(e.r1) ^ F.contains(e.r2)))
					.min(Comparator.comparing(e -> e.dist)).get();
			Q.remove(start.r2);
			Q.remove(start.r1);
			F.add(start.r2);
			F.add(start.r1);
			E.remove(start);
			G.add(start);
			if (start.r1.value < start.r2.value) {
				start.r2.value = (float) (start.r1.value + start.dist);
			} else {
				start.r1.value = (float) (start.r2.value + start.dist);
			}
		}
		return new Pair<>(rooms, new Pair<>(root, G));
	}
}